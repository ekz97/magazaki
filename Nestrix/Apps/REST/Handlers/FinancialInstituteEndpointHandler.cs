using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Peasie.Contracts;
using PeasieLib;
using PeasieLib.Interfaces;
using PeasieLib.Services;
using RESTLayer.Interfaces;

namespace RESTLayer.Handlers
{
    public class FinancialInstituteEndpointHandler
    {
        private readonly Dictionary<string, PaymentWrapper> _paymentHistory = new();

        public void RegisterAPIs(WebApplication app,
            SymmetricSecurityKey key, X509SecurityKey signingCertificateKey, X509SecurityKey encryptingCertificateKey)
        {
            var applicationContextService = app.Services.GetService(typeof(IPeasieApplicationContextService)) as IPeasieApplicationContextService;
            var financialTransactionProcessor = app.Services.GetService(typeof(IPaymentTransactionService)) as IPaymentTransactionService;

            // GROUPS OF ENDPOINTS ==================================================
            var authenticationHandler = app.MapGroup("/token").WithTags("Authentication API");
            var sessionHandler = app.MapGroup("/session").WithTags("Session API");
            var paymentHandler = app.MapGroup("/payment").WithTags("Payment API");
            var adminHandler = app.MapGroup("/admin").WithTags("Admin View");
            // ======================================================================

            // TOKEN ================================================================

            _ = authenticationHandler.MapPost("/rfa", () =>
            {
                applicationContextService?.Logger.LogDebug("-> FinancialInstituteEndpointHandler::Rfa");
                applicationContextService?.GetAuthenticationToken();
                applicationContextService?.Logger.LogDebug("<- FinancialInstituteEndpointHandler::Rfa");
            });

            // SESSION ==============================================================

            _ = sessionHandler.MapPost("/request", (UserDTO userDTO) =>
            {
                applicationContextService?.Logger.LogDebug("-> FinancialInstituteEndpointHandler::SessionRequest");
                var ok = applicationContextService?.GetSession(userDTO) ?? false;
                if (!ok)
                    return Results.BadRequest();
                applicationContextService?.Logger.LogDebug("<- FinancialInstituteEndpointHandler::SessionRequest");
                return Results.Ok();
            });

            // PAYMENT ==============================================================

            _ = paymentHandler.MapPost("/request", (PeasieRequestDTO peasieRequestDTO) =>
            {
                applicationContextService?.Logger.LogDebug("-> FinancialInstituteEndpointHandler::PaymentRequest");
                PaymentRequestDTO? paymentRequest = null;

                // payment request
                string? decrypted = null;
                if (applicationContextService.Session != null && !string.IsNullOrEmpty(applicationContextService.Session.PrivateKey))
                {
                    decrypted = EncryptionService.DecryptUsingPrivateKey(peasieRequestDTO.Payload, applicationContextService.Session.PrivateKey);
                    paymentRequest = JsonSerializer.Deserialize<PaymentRequestDTO>(decrypted);
                }

                if (string.IsNullOrEmpty(decrypted) || string.IsNullOrEmpty(paymentRequest?.BeneficiaryPublicKey))
                {
                    applicationContextService?.Logger.LogDebug("<- FinancialInstituteEndpointHandler::PaymentRequest (BadRequest)");
                    return Results.BadRequest();
                }
               
                // send payment SID
                applicationContextService?.Logger.LogDebug("Sending Payment SID...");

                var paymentSID = Guid.NewGuid();
                TokenService.GeneratePPKRandomly(out string privateKey, out string publicKey);
                var paymentResponseDTO = new PaymentResponseDTO
                {
                    SourceGuid = paymentRequest.Guid.ToString(),
                    PaymentSID = paymentSID.ToString(),
                    ReplyTimeUtc = DateTime.UtcNow,
                    ValidityTimeSpan = new TimeSpan(0, 30, 0),
                    FinancialInstitutePublicKey = publicKey
                };

                var bankJson = JsonSerializer.Serialize<PaymentResponseDTO>(paymentResponseDTO);
                var bankEncryptedResponse = EncryptionService.EncryptUsingPublicKey(bankJson, paymentRequest.BeneficiaryPublicKey);
                var bankReply = new PeasieReplyDTO
                {
                    Payload = bankEncryptedResponse
                };
                var json = JsonSerializer.Serialize(bankReply);
                var encryptedResponse = EncryptionService.EncryptUsingPublicKey(json, applicationContextService.Session.SessionResponse.PublicKey);

                var reply = new PeasieReplyDTO
                {
                    Payload = encryptedResponse
                };

                // remember
                applicationContextService?.Logger.LogDebug("Remembering Payment SID...");

                _paymentHistory[paymentSID.ToString()] = new PaymentWrapper() { Request = paymentRequest, Response = paymentResponseDTO, FinancialInstitutePrivateKey = privateKey }; // private key is of bank!

                applicationContextService?.Logger.LogDebug("<- FinancialInstituteEndpointHandler::PaymentRequest (OK)");
                return Results.Ok(reply);
            }).RequireAuthorization("IsAuthorized");

            _ = paymentHandler.MapPost("/trx", (PeasieRequestDTO peasieRequestDTO) =>
            {
                applicationContextService?.Logger.LogDebug("-> FinancialInstituteEndpointHandler::PaymentTrx");

                PeasieRequestDTO? bankPeasieRequestDTO = null;
                PaymentTransactionDTO? paymentTrx = null;

                string? decrypted = null;
                if (applicationContextService.Session != null && !string.IsNullOrEmpty(applicationContextService.Session.PrivateKey))
                {
                    decrypted = EncryptionService.DecryptUsingPrivateKey(peasieRequestDTO.Payload, applicationContextService.Session.PrivateKey);
                    bankPeasieRequestDTO = JsonSerializer.Deserialize<PeasieRequestDTO>(decrypted);
                }

                if (string.IsNullOrEmpty(decrypted))
                {
                    applicationContextService?.Logger.LogDebug("<- FinancialInstituteEndpointHandler::PaymentTrx (BadRequest)");
                    return Results.BadRequest();
                }

                var paymentRequest = _paymentHistory[bankPeasieRequestDTO.Id];
                var decryptedBankTrx = EncryptionService.DecryptUsingPrivateKey(bankPeasieRequestDTO.Payload, paymentRequest.FinancialInstitutePrivateKey);

                var trx = JsonSerializer.Deserialize<PaymentTransactionDTO>(decryptedBankTrx);

                applicationContextService?.Logger.LogDebug("About to call payment service...");
                if (trx != null)
                {

                    financialTransactionProcessor = app.Services.GetService(typeof(IPaymentTransactionService)) as IPaymentTransactionService;
                    if (financialTransactionProcessor == null)
                    {
                        applicationContextService?.Logger.LogDebug("Payment service NOT AVAILABLE!");
                    }
                    else
                    {
                        applicationContextService?.Logger.LogDebug("Calling payment service...");
                        trx = financialTransactionProcessor?.Process(trx);
                        // trx.Status = "COMPLETED"; // TODO: adapt own history!!
                    }
                }

                var jsonBank = JsonSerializer.Serialize<PaymentTransactionDTO>(trx);

                var encryptedBankResponse = EncryptionService.EncryptUsingPublicKey(jsonBank, paymentRequest.Request.BeneficiaryPublicKey);
 
                var bankReply = new PeasieReplyDTO
                {
                    Payload = encryptedBankResponse
                };

                var json = JsonSerializer.Serialize(bankReply);
                var encryptedResponse = EncryptionService.EncryptUsingPublicKey(json, applicationContextService.Session.SessionResponse.PublicKey);

                var reply = new PeasieReplyDTO
                {
                    Payload = encryptedResponse
                };

                applicationContextService?.Logger.LogDebug("<- FinancialInstituteEndpointHandler::PaymentTrx (OK)");
                return Results.Ok(reply);
            }).RequireAuthorization("IsAuthorized");

            // ADMIN =================================================================

            _ = adminHandler.MapGet("/", () =>
            {
                applicationContextService?.Logger.LogDebug("-> FinancialInstituteEndpointHandler::AdminIndex");
                var html = System.IO.File.ReadAllText(@"./Assets/admin.html");
                StringBuilder htmlContentBuilder = new();
                htmlContentBuilder.Append(applicationContextService?.ToHtml());
                //htmlContentBuilder.Append(_paymentTransactions.IEnumerableToHtmlTable());
                html = html.Replace("{{placeholder}}", htmlContentBuilder.ToString());
                applicationContextService?.Logger.LogDebug("<- FinancialInstituteEndpointHandler::AdminIndex");
                return Results.Extensions.Html(html);
            });
        }
    }
}
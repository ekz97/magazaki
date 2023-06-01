using Flurl.Http;
using Microsoft.IdentityModel.Tokens;
using Peasie.Contracts;
using PeasieLib.Interfaces;
using PeasieLib.Services;
using System.Text.Json;
using PeasieLib;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace WebshopApi.Infrastructure.Handlers
{

    public class BeneficiaryEndpointHandler
    {
        //private SessionRequestDTOWrapper _session = new();
        private static readonly Dictionary<string, PaymentTrxWrapper?> _paymentTransactions = new();

        public void RegisterAPIs(WebApplication app,
            SymmetricSecurityKey key, X509SecurityKey signingCertificateKey, X509SecurityKey encryptingCertificateKey)
        {
            var applicationContextService = app.Services.GetService(typeof(IPeasieApplicationContextService)) as IPeasieApplicationContextService;

            // GROUPS OF ENDPOINTS ==================================================
            var tokenHandler = app.MapGroup("/token").WithTags("Authentication API");
            var sessionHandler = app.MapGroup("/session").WithTags("Session API");
            var paymentHandler = app.MapGroup("/payment").WithTags("Payment API");
            var hookHandler = app.MapGroup("/hook").WithTags("WebHook API");
            var adminHandler = app.MapGroup("/admin").WithTags("Admin View");
            // ======================================================================

            // TOKEN ================================================================

            _ = tokenHandler.MapPost("/rfa", () =>
            {
                applicationContextService?.Logger.LogDebug("-> BeneficiaryEndpointHandler::Rfa");
                applicationContextService?.GetAuthenticationToken();
                applicationContextService?.Logger.LogDebug("<- BeneficiaryEndpointHandler::Rfa");
            });

            // SESSION ==============================================================

            _ = sessionHandler.MapPost("/request", (UserDTO userDTO) =>
            {
                applicationContextService?.Logger.LogDebug("-> BeneficiaryEndpointHandler::SessionRequest");
                var ok = applicationContextService?.GetSession(userDTO) ?? false;
                if (!ok)
                    return Results.BadRequest();
                applicationContextService?.Logger.LogDebug("<- BeneficiaryEndpointHandler::SessionRequest");
                return Results.Ok();
            });

            // HOOK =================================================================

            _ = hookHandler.MapPost("/PaymentTrxUpdate", () =>
            {
                applicationContextService?.Logger.LogDebug("-> BeneficiaryEndpointHandler::PaymentTrxUpdate");
                // Decrypt using session public key
                applicationContextService?.Logger.LogDebug("<- BeneficiaryEndpointHandler::PaymentTrxUpdate");
                return Results.Ok();
            }).RequireAuthorization("IsAuthorized"); ;

            // PAYMENT ==============================================================

            _ = paymentHandler.MapPost("/request", (PaymentTrxDTO paymentParameters) =>
            {
                applicationContextService?.Logger.LogDebug("-> BeneficiaryEndpointHandler::PaymentRequest");
                if (!MakePaymentRequest(applicationContextService, paymentParameters))
                    return Results.BadRequest();
                applicationContextService?.Logger.LogDebug("<- BeneficiaryEndpointHandler::PaymentRequest");
                return Results.Ok();
            }).RequireAuthorization("IsAuthorized");

            // ADMIN =================================================================

            _ = adminHandler.MapGet("/", () =>
            {
                applicationContextService?.Logger.LogDebug("-> BeneficiaryEndpointHandler::AdminIndex");
                var html = System.IO.File.ReadAllText(@"./Assets/admin.html");
                StringBuilder htmlContentBuilder = new();
                htmlContentBuilder.Append(applicationContextService?.ToHtml());
                htmlContentBuilder.Append(_paymentTransactions.IEnumerableToHtmlTable());
                html = html.Replace("{{placeholder}}", htmlContentBuilder.ToString());
                applicationContextService?.Logger.LogDebug("<- BeneficiaryEndpointHandler::AdminIndex");
                return Results.Extensions.Html(html);
            });
        }

        public static bool MakePaymentRequest(IPeasieApplicationContextService? applicationContextService, PaymentTrxDTO paymentParameters)
        {
            applicationContextService?.Logger.LogDebug("-> BeneficiaryEndpointHandler::MakePaymentRequest");

            var valid = applicationContextService != null
                && applicationContextService.AuthenticationToken != null
                && applicationContextService.Session != null
                && !string.IsNullOrEmpty(applicationContextService.Session.PrivateKey);

            if (valid)
            {
                applicationContextService?.Logger.LogDebug("Requesting Payment SID...");
                PaymentResponseDTO? paymentResponseDTO;
                string? paymentRequestPrivateKeyBeneficiary;
                {
                    var rsaPaymentRequest = TokenService.GeneratePPKRandomly(out paymentRequestPrivateKeyBeneficiary, out string? paymentRequestPublicKeyBeneficiary);
                    var paymentRequest = new PaymentRequestDTO() { BeneficiaryPublicKey = paymentRequestPublicKeyBeneficiary, SessionDetails = applicationContextService.Session.SessionDetails };

                    var json = JsonSerializer.Serialize<PaymentRequestDTO>(paymentRequest);
                    var encrypted = EncryptionService.EncryptUsingPublicKey(json, applicationContextService.Session.SessionResponse.PublicKey);
                    var peasieRequestDTO = new PeasieRequestDTO { Id = applicationContextService.Session.SessionResponse.SessionGuid.ToString(), Payload = encrypted };

                    var url = applicationContextService.PeasieUrl + "/payment/request";
                    var reference = url.WithOAuthBearerToken(applicationContextService.AuthenticationToken).PostJsonAsync(peasieRequestDTO).Result;
                    var reply = reference.GetJsonAsync<PeasieReplyDTO>().Result;
                    if (string.IsNullOrEmpty(reply.Payload))
                        return false;

                    var decrypted = EncryptionService.DecryptUsingPrivateKey(reply.Payload, applicationContextService.Session.PrivateKey);
                    if (string.IsNullOrEmpty(decrypted)) return false;
                    var paymentRequestReplyDTO = System.Text.Json.JsonSerializer.Deserialize<PeasieReplyDTO>(decrypted);

                    var bankDecrypted = EncryptionService.DecryptUsingPrivateKey(paymentRequestReplyDTO.Payload, paymentRequestPrivateKeyBeneficiary);
                    if (string.IsNullOrEmpty(decrypted)) return false;

                    paymentResponseDTO = System.Text.Json.JsonSerializer.Deserialize<PaymentResponseDTO>(bankDecrypted);
                }

                // Use the SID to request the payment transaction immediately
                applicationContextService?.Logger.LogDebug("Sending Payment TRX...");
                if (paymentResponseDTO != null)
                {
                    applicationContextService?.Logger.LogDebug($"PaymentSID: {paymentResponseDTO.PaymentSID}");

                    PaymentTransactionDTO paymentTrx = new(
                        id: paymentResponseDTO.PaymentSID.ToString(),
                        shortId: "",
                        accountId: "",
                        createdDate: DateTime.Now,
                        updatedDate: DateTime.Now,
                        paymentDate: DateTime.Now,
                        transactionId: paymentParameters.TransactionId,
                        transactionCategory: "",
                        paymentType: "",
                        type: "",
                        sourceInfo: new SourceInfoDTO(type: "", identifier: "", internalAccountId: ""), // sourceInfo,
                        destinationInfo: new DestinationInfoDTO(type: "", identifier: ""), // destinationInfo,
                        source: null,
                        destination: "",
                        totalAmount: new AmountDTO(paymentParameters.Amount, paymentParameters.Currency),
                        amount: new AmountDTO(paymentParameters.Amount, paymentParameters.Currency),
                        fee: new AmountDTO(0, paymentParameters.Currency),
                        runningBalance: new AmountDTO(0, paymentParameters.Currency),
                        buyAmount: null,
                        fxRate: null,
                        midMarketRate: null,
                        fixedSide: null,
                        status: "INITIATED",
                        failureReason: null,
                        comment: paymentParameters.Comment,
                        transactionReference: null,
                        referenceAmount: null,
                        accountHolderId: ""
                    );

                    applicationContextService?.Logger.LogDebug("Encrypting using public key of bank");
                    var json = JsonSerializer.Serialize<PaymentTransactionDTO>(paymentTrx);
                    var encryptedTrx = EncryptionService.EncryptUsingPublicKey(json, paymentResponseDTO.FinancialInstitutePublicKey);

                    PeasieRequestDTO wrappedTrx = new()
                    {
                        Id = paymentResponseDTO.PaymentSID,
                        Payload = encryptedTrx
                    };

                    applicationContextService?.Logger.LogDebug("Encrypting using public key of Peasie session");
                    var jsonWrappedTrx = JsonSerializer.Serialize<PeasieRequestDTO>(wrappedTrx);
                    var encrypted = EncryptionService.EncryptUsingPublicKey(jsonWrappedTrx, applicationContextService.Session.SessionResponse.PublicKey);

                    applicationContextService?.Logger.LogDebug("Sending TRX...");
                    var peasieRequestDTO = new PeasieRequestDTO { Id = applicationContextService.Session.SessionResponse.SessionGuid.ToString(), Payload = encrypted };
                    var url = applicationContextService.PeasieUrl + "/payment/trx";
                    var reference = url.WithOAuthBearerToken(applicationContextService.AuthenticationToken).PostJsonAsync(peasieRequestDTO).Result;

                    applicationContextService?.Logger.LogDebug("Receiving TRX response...");

                    var reply = reference.GetJsonAsync<PeasieReplyDTO>().Result;
                    if (string.IsNullOrEmpty(reply.Payload))
                    {
                        applicationContextService?.Logger.LogDebug("<- BeneficiaryEndpointHandler::MakePaymentRequest (payload error)");
                        return false;
                    }

                    var decrypted = EncryptionService.DecryptUsingPrivateKey(reply.Payload, applicationContextService.Session.PrivateKey);
                    if (string.IsNullOrEmpty(decrypted))
                    {
                        applicationContextService?.Logger.LogDebug("<- BeneficiaryEndpointHandler::MakePaymentRequest (decryption error)");
                        return false;
                    }
                    var paymentTransactionReplyDTO = System.Text.Json.JsonSerializer.Deserialize<PeasieReplyDTO>(decrypted);

                    var decryptedTrxResponse = EncryptionService.DecryptUsingPrivateKey(paymentTransactionReplyDTO.Payload, paymentRequestPrivateKeyBeneficiary);

                    if (string.IsNullOrEmpty(decryptedTrxResponse))
                    {
                        applicationContextService?.Logger.LogDebug("<- BeneficiaryEndpointHandler::MakePaymentRequest (TRX decryption error)");
                        return false;
                    }

                    var paymentTrxResponse = System.Text.Json.JsonSerializer.Deserialize<PaymentTransactionDTO>(decryptedTrxResponse);

                    applicationContextService?.Logger.LogDebug($"Payment TRX status: {paymentTrxResponse.Status} ({paymentTrxResponse.Amount.Value} {paymentTrxResponse.Amount.Currency})");

                    applicationContextService?.Logger.LogDebug("Remembering TRX...");
                    // TODO: compare to trx already stored...

                    _paymentTransactions[paymentResponseDTO.PaymentSID.ToString()] = new PaymentTrxWrapper() { Request = paymentTrx, Response = paymentResponseDTO };
                    _paymentTransactions[paymentResponseDTO.PaymentSID.ToString()].Updates.Add(paymentTrxResponse);

                    applicationContextService?.Logger.LogDebug("<- BeneficiaryEndpointHandler::MakePaymentRequest");
                }
            }

            applicationContextService?.Logger.LogDebug("<- BeneficiaryEndpointHandler::MakePaymentRequest");
            return valid;
        }
    }
}
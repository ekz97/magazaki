using Flurl.Http;
using Microsoft.IdentityModel.Tokens;
using Peasie.Contracts;
using PeasieLib.Services;
using System.Text.Json;
using PeasieLib.Models.DTO;
using PeasieLib.Interfaces;

namespace FinancialInstituteAPI.Handlers
{
    public class FinancialInstituteEndpointHandler
    {
        private readonly Dictionary<Guid, PaymentWrapper> _paymentHistory = new();

        public void RegisterAPIs(WebApplication app,
            SymmetricSecurityKey key, X509SecurityKey signingCertificateKey, X509SecurityKey encryptingCertificateKey)
        {
            var applicationContextService = app.Services.GetService(typeof(IPeasieApplicationContextService)) as IPeasieApplicationContextService;

            // GROUPS OF ENDPOINTS ==================================================
            var authenticationHandler = app.MapGroup("/token").WithTags("Authentication API");
            var sessionHandler = app.MapGroup("/session").WithTags("Session API");
            var paymentHandler = app.MapGroup("/payment").WithTags("Payment API");
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
                    return Results.BadRequest();
               
                // send payment SID
                var paymentSID = Guid.NewGuid();
                TokenService.GeneratePPKRandomly(out string privateKey, out string publicKey);
                var paymentResponseDTO = new PaymentResponseDTO
                {
                    SourceGuid = paymentRequest.Guid,
                    PaymentSID = paymentSID,
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
                _paymentHistory[paymentSID] = new PaymentWrapper() { PaymentRequest = paymentRequest, PaymentResponse = paymentResponseDTO, FinancialInstitutePrivateKey = privateKey }; // private key is of bank!

                applicationContextService?.Logger.LogDebug("<- FinancialInstituteEndpointHandler::PaymentRequest");
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
                    return Results.BadRequest();

                var paymentRequest = _paymentHistory[bankPeasieRequestDTO.Id];
                var decryptedBankTrx = EncryptionService.DecryptUsingPrivateKey(bankPeasieRequestDTO.Payload, paymentRequest.FinancialInstitutePrivateKey);

                var bankTrx = JsonSerializer.Deserialize<PaymentTransactionDTO>(decryptedBankTrx);

                bankTrx.Status = "COMPLETED"; // TODO: adapt own history!!

                var jsonBank = JsonSerializer.Serialize<PaymentTransactionDTO>(bankTrx);

                var encryptedBankResponse = EncryptionService.EncryptUsingPublicKey(jsonBank, paymentRequest.PaymentRequest.BeneficiaryPublicKey);
 
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

                applicationContextService?.Logger.LogDebug("<- FinancialInstituteEndpointHandler::PaymentTrx");
                return Results.Ok(reply);
            }).RequireAuthorization("IsAuthorized");
        }

        /*
        public static void GetAuthenticationToken(IPeasieApplicationContextService? applicationContextService)
        {
            var url = applicationContextService?.PeasieUrl + "/token/generateEncryptedToken";
            applicationContextService.AuthenticationToken = url.GetStringAsync().Result;
        }

        public static bool GetSession(IPeasieApplicationContextService? applicationContextService, UserDTO userDTO)
        {
            // request session key
            var rsa = TokenService.GeneratePPKRandomly(out string privateKey, out string publicKey);
            var sessionRequest = new SessionRequestDTO
            {
                PublicKey = publicKey
            };
            var url = applicationContextService?.PeasieUrl + "/session/request";
            var reference = url.WithOAuthBearerToken(applicationContextService?.AuthenticationToken).PostJsonAsync(sessionRequest).Result;
            var reply = reference.GetJsonAsync<PeasieReplyDTO>().Result;
            if (string.IsNullOrEmpty(reply.Payload))
                return false;
            var decrypted = EncryptionService.DecryptUsingPrivateKey(reply.Payload, privateKey);
            var sessionResponse = System.Text.Json.JsonSerializer.Deserialize<SessionResponseDTO>(decrypted);

            // remember
            var wrapper = new SessionRequestDTOWrapper { SessionRequest = sessionRequest, SessionResponse = sessionResponse, PrivateKey = privateKey, PublicKey = publicKey };
            applicationContextService.Session = wrapper;

            if (applicationContextService.Session == null || applicationContextService.Session.SessionResponse == null)
                return false;

            // send session details
            var sessionDetailsDTO = new SessionDetailsDTO() { Guid = applicationContextService.Session.SessionResponse.SessionGuid, User = userDTO, Issuer = applicationContextService.Issuer, Audience = applicationContextService.Audience, WebHook = applicationContextService.WebHook, JWTAuthorizationToken = applicationContextService.AuthenticationToken };
            var json = JsonSerializer.Serialize<SessionDetailsDTO>(sessionDetailsDTO);
            var encrypted = EncryptionService.EncryptUsingPublicKey(json, applicationContextService.Session.SessionResponse.PublicKey);
            var peasieRequestDTO = new PeasieRequestDTO { Id = applicationContextService.Session.SessionResponse.SessionGuid, Payload = encrypted };
            url = applicationContextService.PeasieUrl + "/session/details";
            reference = url.WithOAuthBearerToken(applicationContextService.AuthenticationToken).PostJsonAsync(peasieRequestDTO).Result;

            // remember
            applicationContextService.Session.SessionDetails = sessionDetailsDTO;

            return true;
        }
        */
    }
}
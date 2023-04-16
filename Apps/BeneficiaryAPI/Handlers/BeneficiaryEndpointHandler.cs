using BeneficiaryAPI.Interfaces;
using Flurl.Http;
using Microsoft.IdentityModel.Tokens;
using Peasie.Contracts;
using PeasieLib.Models.DTO;
using PeasieLib.Services;
using System.Text.Json;

namespace BeneficiaryAPI.Handlers
{

    public class BeneficiaryEndpointHandler
    {
        private SessionRequestDTOWrapper _session = new();
        private static readonly Dictionary<Guid, PaymentTrxWrapper> _paymentTransactions = new();

        public void RegisterAPIs(WebApplication app,
            SymmetricSecurityKey key, X509SecurityKey signingCertificateKey, X509SecurityKey encryptingCertificateKey)
        {
            var applicationContextService = app.Services.GetService(typeof(IApplicationContextService)) as IApplicationContextService;

            // GROUPS OF ENDPOINTS ==================================================
            var tokenHandler = app.MapGroup("/token").WithTags("Authentication API");
            var sessionHandler = app.MapGroup("/session").WithTags("Session API");
            var paymentHandler = app.MapGroup("/payment").WithTags("Payment API");
            var hookHandler = app.MapGroup("/hook").WithTags("WebHook API");
            // ======================================================================

            // TOKEN ================================================================

            _ = tokenHandler.MapPost("/rfa", () =>
            {
                applicationContextService?.Logger.LogDebug("-> BeneficiaryEndpointHandler::Rfa");
                GetAuthenticationToken(applicationContextService);
                applicationContextService?.Logger.LogDebug("<- BeneficiaryEndpointHandler::Rfa");
            });

            // SESSION ==============================================================

            _ = sessionHandler.MapPost("/request", (UserDTO userDTO) =>
            {
                applicationContextService?.Logger.LogDebug("-> BeneficiaryEndpointHandler::SessionRequest");
                if (!GetSession(applicationContextService, userDTO))
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
            });

            // PAYMENT ==============================================================

            _ = paymentHandler.MapPost("/request", (PaymentTrxDTO paymentParameters) =>
            {
                applicationContextService?.Logger.LogDebug("-> BeneficiaryEndpointHandler::PaymentRequest");
                if(!MakePaymentRequest(applicationContextService, paymentParameters)) 
                    return Results.BadRequest();
                applicationContextService?.Logger.LogDebug("<- BeneficiaryEndpointHandler::PaymentRequest");
                return Results.Ok();
            });
        }

        public static void GetAuthenticationToken(IApplicationContextService? applicationContextService)
        {
            var url = applicationContextService?.PeasieUrl + "/token/generateEncryptedToken";
            applicationContextService.AuthenticationToken = url.GetStringAsync().Result;
        }

        public static bool GetSession(IApplicationContextService? applicationContextService, UserDTO userDTO)
        {
            // request session key
            var rsa = TokenService.GeneratePPKRandomly(out string privateKey, out string publicKey);
            var sessionRequest = new SessionRequestDTO
            {
                PublicKey = publicKey
            };
            var url = applicationContextService?.PeasieUrl + "/session/request";
            var reference = url.WithOAuthBearerToken(applicationContextService.AuthenticationToken).PostJsonAsync(sessionRequest).Result;
            var reply = reference.GetJsonAsync<PeasieReplyDTO>().Result;
            if (string.IsNullOrEmpty(reply.Payload))
                return false;
            var decrypted = EncryptionService.DecryptUsingPrivateKey(reply.Payload, privateKey);
            var sessionResponse = System.Text.Json.JsonSerializer.Deserialize<SessionResponseDTO>(decrypted);

            // remember
            var wrapper = new SessionRequestDTOWrapper { SessionRequest = sessionRequest, SessionResponse = sessionResponse, RSA = rsa, PrivateKey = privateKey, PublicKey = publicKey };
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

        public static bool MakePaymentRequest(IApplicationContextService? applicationContextService, PaymentTrxDTO paymentParameters)
        {
            PaymentResponseDTO? paymentResponseDTO;
            string? paymentRequestPrivateKeyBeneficiary;
            {
                if (applicationContextService.Session == null || string.IsNullOrEmpty(applicationContextService.Session.PrivateKey))
                {
                    return false;
                }
                var rsaPaymentRequest = TokenService.GeneratePPKRandomly(out paymentRequestPrivateKeyBeneficiary, out string? paymentRequestPublicKeyBeneficiary);
                var paymentRequest = new PaymentRequestDTO() { BeneficiaryPublicKey = paymentRequestPublicKeyBeneficiary, SessionDetails = applicationContextService.Session.SessionDetails };

                var json = JsonSerializer.Serialize<PaymentRequestDTO>(paymentRequest);
                var encrypted = EncryptionService.EncryptUsingPublicKey(json, applicationContextService.Session.SessionResponse.PublicKey);
                var peasieRequestDTO = new PeasieRequestDTO { Id = applicationContextService.Session.SessionResponse.SessionGuid, Payload = encrypted };

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
            if (paymentResponseDTO != null)
            {
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

                // Encrypt using public key of bank
                var json = JsonSerializer.Serialize<PaymentTransactionDTO>(paymentTrx);
                var encryptedTrx = EncryptionService.EncryptUsingPublicKey(json, paymentResponseDTO.FinancialInstitutePublicKey);

                PeasieRequestDTO wrappedTrx = new()
                {
                    Id = paymentResponseDTO.PaymentSID,
                    Payload = encryptedTrx
                };
                // Encrypt using public key of Peasie session
                var jsonWrappedTrx = JsonSerializer.Serialize<PeasieRequestDTO>(wrappedTrx);
                var encrypted = EncryptionService.EncryptUsingPublicKey(jsonWrappedTrx, applicationContextService.Session.SessionResponse.PublicKey);

                //_logger.LogDebug(sessionSymmetricPwdEnc.HexDump());

                // Send
                var peasieRequestDTO = new PeasieRequestDTO { Id = applicationContextService.Session.SessionResponse.SessionGuid, Payload = encrypted };
                var url = applicationContextService.PeasieUrl + "/payment/trx";
                var reference = url.WithOAuthBearerToken(applicationContextService.AuthenticationToken).PostJsonAsync(peasieRequestDTO).Result;

                var reply = reference.GetJsonAsync<PeasieReplyDTO>().Result;
                if (string.IsNullOrEmpty(reply.Payload))
                    return false;

                var decrypted = EncryptionService.DecryptUsingPrivateKey(reply.Payload, applicationContextService.Session.PrivateKey);
                if (string.IsNullOrEmpty(decrypted)) return false;
                var paymentTransactionReplyDTO = System.Text.Json.JsonSerializer.Deserialize<PeasieReplyDTO>(decrypted);

                var decryptedTrxResponse = EncryptionService.DecryptUsingPrivateKey(paymentTransactionReplyDTO.Payload, paymentRequestPrivateKeyBeneficiary);

                //_logger.LogDebug(bankSymmetricPwdDec.HexDump());

                var paymentTrxResponse = System.Text.Json.JsonSerializer.Deserialize<PaymentTransactionDTO>(decryptedTrxResponse);

                applicationContextService?.Logger.LogDebug($"Payment TRX status: {paymentTrxResponse.Status} ({paymentTrxResponse.Amount.Value} {paymentTrxResponse.Amount.Currency})");

                // TODO: compare to trx already stored...

                _paymentTransactions[paymentResponseDTO.PaymentSID] = new PaymentTrxWrapper() { PaymentTrxRequest = paymentTrx, PaymentResponseDTO = paymentResponseDTO };
                _paymentTransactions[paymentResponseDTO.PaymentSID].PaymentTrxReplyies.Add(paymentTrxResponse);
            }
            return true;
        }
    }
}
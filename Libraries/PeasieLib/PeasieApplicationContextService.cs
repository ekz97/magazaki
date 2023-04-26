using Flurl.Http;
using Microsoft.Extensions.Logging;
using Peasie.Contracts;
using PeasieLib.Interfaces;
using PeasieLib.Models.DTO;
using PeasieLib.Services;
using System.Text.Json;

namespace PeasieLib
{
    public class PeasieApplicationContextService : IPeasieApplicationContextService
    {
        public ILogger? Logger { get; set; }
        public string? PeasieUrl { get; set; }
        public SessionRequestDTOWrapper? Session { get; set; }
        public string? AuthenticationToken { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? WebHook { get; set; }
        public bool? DemoMode { get; set; }
        public string? PeasieClientId { get; set; } // email registered in identity
        public string? PeasieClientSecret { get; set; } // password registered in identity  

        public bool GetAuthenticationToken()
        {
            bool valid = true;
            // TODO: check secret in identity db
            if (valid)
            {
                var url = PeasieUrl + "/token/generateEncryptedToken";
                AuthenticationToken = url.GetStringAsync().Result;
            }
            return valid;
        }

        public bool GetSession(UserDTO userDTO)
        {
            bool valid = true;
            // TODO: check secret in identity db
            if (valid)
            {
                // request session key
                var rsa = TokenService.GeneratePPKRandomly(out string privateKey, out string publicKey);
                var sessionRequest = new SessionRequestDTO
                {
                    PublicKey = publicKey
                };
                var url = PeasieUrl + "/session/request";
                var reference = url.WithOAuthBearerToken(AuthenticationToken).PostJsonAsync(sessionRequest).Result;
                var reply = reference.GetJsonAsync<PeasieReplyDTO>().Result;
                if (string.IsNullOrEmpty(reply.Payload))
                    return false;
                var decrypted = EncryptionService.DecryptUsingPrivateKey(reply.Payload, privateKey);
                var sessionResponse = System.Text.Json.JsonSerializer.Deserialize<SessionResponseDTO>(decrypted);

                // remember
                var wrapper = new SessionRequestDTOWrapper { SessionRequest = sessionRequest, SessionResponse = sessionResponse, RSA = rsa, PrivateKey = privateKey, PublicKey = publicKey };
                Session = wrapper;

                if (Session == null || Session.SessionResponse == null)
                    return false;

                // send session details
                var sessionDetailsDTO = new SessionDetailsDTO() { Guid = Session.SessionResponse.SessionGuid, User = userDTO, Issuer = Issuer, Audience = Audience, WebHook = WebHook, JWTAuthorizationToken = AuthenticationToken };
                var json = JsonSerializer.Serialize<SessionDetailsDTO>(sessionDetailsDTO);
                var encrypted = EncryptionService.EncryptUsingPublicKey(json, Session.SessionResponse.PublicKey);
                var peasieRequestDTO = new PeasieRequestDTO { Id = Session.SessionResponse.SessionGuid, Payload = encrypted };
                url = PeasieUrl + "/session/details";
                reference = url.WithOAuthBearerToken(AuthenticationToken).PostJsonAsync(peasieRequestDTO).Result;

                // remember
                Session.SessionDetails = sessionDetailsDTO;
            }
            return valid;
        }
    }
}
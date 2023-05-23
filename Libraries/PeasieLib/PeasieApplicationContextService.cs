using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Peasie.Contracts;
using PeasieLib.Interfaces;
using PeasieLib.Models.DTO;
using PeasieLib.Services;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace PeasieLib
{
    // https://github.com/sebastienros/memoryleak/blob/master/src/MemoryLeak/MemoryLeak/Controllers/DiagnosticsController.cs

    public class PeasieApplicationContextService : IPeasieApplicationContextService
    {
        #region Properties
        public ILogger? Logger { get; set; }
        public string? ConnectionString { get; set; }
        public string? PeasieUrl { get; set; }
        public string? FinancialInstituteUrl { get; set; }
        public SessionRequestDTOWrapper? Session { get; set; }
        public string? AuthenticationToken { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? WebHook { get; set; }
        public bool? DemoMode { get; set; }
        public string? PeasieClientId { get; set; } // email registered in identity
        public string? PeasieClientSecret { get; set; } // password registered in identity  
        public SymmetricSecurityKey? SymmetricKey { get; set; }

        public TimeSpan Lifetime { get; set; }

        public System.Security.Cryptography.X509Certificates.X509Certificate2? SigningCertificate { get; set; }
        public System.Security.Cryptography.X509Certificates.X509Certificate2? EncryptingCertificate { get; set; }
        public X509SecurityKey? SigningCertificateKey { get; set; }
        public X509SecurityKey? EncryptingCertificateKey { get; set; }

        private static readonly Process _process = Process.GetCurrentProcess();
        private static TimeSpan _oldCPUTime = TimeSpan.Zero;
        private static DateTime _lastMonitorTime = DateTime.UtcNow;
        private static DateTime _lastRpsTime = DateTime.UtcNow;
        private static double _cpu = 0, _rps = 0;
        private static readonly double RefreshRate = TimeSpan.FromSeconds(10).TotalMilliseconds;
        public static long Requests = 0;
        #endregion

        #region Methods
        public bool GetAuthenticationToken()
        {
            bool valid = false;
            // TODO: check secret in identity db
            var url = PeasieUrl + "/token/generateEncryptedToken";
            Logger?.LogTrace("Requesting authentication token...");
            try
            {
                AuthenticationToken = url.GetStringAsync().Result;
                Logger?.LogTrace("AuthenticationToken: {0}", AuthenticationToken);
                if (AuthenticationToken != null && !string.IsNullOrEmpty(AuthenticationToken))
                {
                    valid = true;
                }
            }catch( Exception ex )
            {
                Logger?.LogError($"Cannot get authentication token ({ex.Message})", ex);
                valid = false;
            }
            return valid;
        }

        public bool GetSession(UserDTO userDTO)
        {
            if (AuthenticationToken == null)
            {
                Logger?.LogError("Cannot request session: no authentication token");
                return false;
            }

            bool valid = false;
            // TODO: check secret in identity db

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
            var peasieRequestDTO = new PeasieRequestDTO { Id = Session.SessionResponse.SessionGuid.ToString(), Payload = encrypted };
            url = PeasieUrl + "/session/details";
            reference = url.WithOAuthBearerToken(AuthenticationToken).PostJsonAsync(peasieRequestDTO).Result;

            // remember
            Session.SessionDetails = sessionDetailsDTO;

            valid = true;

            return valid;
        }

        // TODO: use ToHtmlTable()
        public string ToHtml()
        {
            StringBuilder htmlStringBuilder = new();
            htmlStringBuilder.Append("<h2>Context</h2>");
            htmlStringBuilder.Append("<table>");
            htmlStringBuilder.Append("<tr><td>Connection string: </td><td>" + PeasieUrl + "</td></tr>");
            htmlStringBuilder.Append("<tr><td>Peasie url:</td><td>" + PeasieUrl + "</td></tr>");
            htmlStringBuilder.Append("<tr><td>Authentication token:</td><td>" + AuthenticationToken + "</td></tr>");
            htmlStringBuilder.Append("<tr><td>Issuer:</td><td>" + Issuer + "</td></tr>");
            htmlStringBuilder.Append("<tr><td>Audience:</td><td>" + Audience + "</td></tr>");
            htmlStringBuilder.Append("<tr><td>WebHook:</td><td>" + WebHook + "</td></tr>");
            htmlStringBuilder.Append("<tr><td>Demo mode:</td><td>" + (DemoMode != null && DemoMode == true ? "Y" : "N") + "</td></tr>");
            htmlStringBuilder.Append("<tr><td>Peasie Client Id:</td><td>" + PeasieClientId + "</td></tr>");
            htmlStringBuilder.Append("<tr><td>Peasie Client Secret:</td><td>" + PeasieClientSecret + "</td></tr>");
            htmlStringBuilder.Append("</table>");

            var statistics = GetDiagnostics();
            var html = PeasieLib.HTMLTableHelper.ParameterToHtmlTable(statistics);

            htmlStringBuilder.Append("<h2>Performance</h2>");
            htmlStringBuilder.Append(html);
            return htmlStringBuilder.ToString();
        }
        #endregion

        public static void GetCollect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public static Diagnostics GetDiagnostics()
        {
            var now = DateTime.UtcNow;
            _process.Refresh();

            var cpuElapsedTime = now.Subtract(_lastMonitorTime).TotalMilliseconds;

            if (cpuElapsedTime > RefreshRate)
            {
                var newCPUTime = _process.TotalProcessorTime;
                var elapsedCPU = (newCPUTime - _oldCPUTime).TotalMilliseconds;
                _cpu = elapsedCPU * 100 / Environment.ProcessorCount / cpuElapsedTime;

                _lastMonitorTime = now;
                _oldCPUTime = newCPUTime;
            }

            var rpsElapsedTime = now.Subtract(_lastRpsTime).TotalMilliseconds;
            if (rpsElapsedTime > RefreshRate)
            {
                _rps = Requests * 1000 / rpsElapsedTime;
                Interlocked.Exchange(ref Requests, 0);
                _lastRpsTime = now;
            }

            var diagnostics = new Diagnostics(
                _process.Id,
                GC.GetTotalMemory(false),
                _process.WorkingSet64,
                _process.PrivateMemorySize64,
                GC.CollectionCount(0),
                GC.CollectionCount(1),
                GC.CollectionCount(2),
                _cpu,
                _rps
            );

            return diagnostics;
        }
    }
}
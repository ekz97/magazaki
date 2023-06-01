using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Peasie.Contracts;
using PeasieLib.Models.DTO;

namespace PeasieLib.Interfaces
{
    public interface IPeasieApplicationContextService
    {
        #region Properties
        public ILogger? Logger { get; set; }
        public IConfiguration? Configuration { get; set; }
        public string? PeasieUrl { get; set; }
        public string? FinancialInstituteUrl { get; set; }
        public SessionRequestDTOWrapper? Session { get; set; }
        public string? AuthenticationToken { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? WebHook { get; set; }
        public string? PeasieClientId { get; set; } // email registered in identity
        public string? PeasieClientSecret { get; set; } // password registered in identity  
        public SymmetricSecurityKey? SymmetricKey { get; set; }
        public TimeSpan Lifetime { get; set; }

        public X509SecurityKey? SigningCertificateKey { get; set; }
        public X509SecurityKey? EncryptingCertificateKey { get; set; }
        #endregion

        #region Service Methods
        public bool GetAuthenticationToken();
        public bool GetSession(UserDTO userDTO);
        #endregion

        #region Methods
        public string ToHtml();
        #endregion
    }
}
using BeneficiaryAPI.Interfaces;
using PeasieLib.Models.DTO;

namespace BeneficiaryAPI
{
        public class ApplicationContextService : IApplicationContextService
        {
            public ILogger? Logger { get; set; }
            public string? PeasieUrl { get; set; }
            public SessionRequestDTOWrapper? Session { get; set; }
            public string? AuthenticationToken { get; set; }
            public string? Issuer { get; set; }
            public string? Audience { get; set; }
            public string? WebHook { get; set; }
            public bool? DemoMode { get; set; }
        }
    }
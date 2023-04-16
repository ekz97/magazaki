using PeasieLib.Models.DTO;

namespace FinancialInstituteAPI.Interfaces
{
    public interface IApplicationContextService
    {
        public ILogger Logger { get; }
        public string? PeasieUrl { get; set; }
        public SessionRequestDTOWrapper? Session { get; set; }
        public string? AuthenticationToken { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? WebHook { get; set; }
    }
}
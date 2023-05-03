using Peasie.Contracts.Interfaces;

namespace Peasie.Contracts
{
    public class SessionDetailsDTO : IToHtmlTable
    {
        public Version Version { get; set; } = new Version(1, 0, 0, 0);
        public Guid Guid { get; set; }
        public UserDTO? User { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? WebHook { get; set; }
        public string? JWTAuthorizationToken { get; set; }
    }
}
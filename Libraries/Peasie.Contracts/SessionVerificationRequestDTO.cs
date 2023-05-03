using Peasie.Contracts.Interfaces;

namespace Peasie.Contracts
{
    public class SessionVerificationRequestDTO : IToHtmlTable
    {
        public Version Version { get; set; } = new Version(1, 0, 0, 0);
    }
}
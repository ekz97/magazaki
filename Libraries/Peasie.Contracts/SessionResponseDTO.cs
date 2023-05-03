using Peasie.Contracts.Interfaces;

namespace Peasie.Contracts
{
    public class SessionResponseDTO : IToHtmlTable
    {
        public Guid SessionGuid { get; set; }
        public DateTime ReplyTimeUtc { get; set; }
        public TimeSpan ValidityTimeSpan { get; set; }
        public string? PublicKey { get; set; }
    }
}
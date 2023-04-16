namespace Peasie.Contracts
{
    public class SessionResponseDTO
    {
        public Guid SessionGuid { get; set; }
        public DateTime ReplyTimeUtc { get; set; }
        public TimeSpan ValidityTimeSpan { get; set; }
        public string? PublicKey { get; set; }
    }
}
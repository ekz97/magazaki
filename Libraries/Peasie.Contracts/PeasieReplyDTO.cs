namespace Peasie.Contracts
{
    // Generic "anonymous" reply packaging specific replies
    public class PeasieReplyDTO
    {
        public Version Version { get; set; } = new Version(1, 0, 0, 0);
        public string? Payload { get; set; }
    }
}
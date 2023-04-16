namespace Peasie.Contracts
{

    public class PeasieRequestDTO
    {
        public Version Version { get; set; } = new Version(1, 0, 0, 0);
        public Guid Id { get; set; }
        public string? Payload { get; set; }
    }
}
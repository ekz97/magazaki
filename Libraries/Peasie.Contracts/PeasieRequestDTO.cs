using Peasie.Contracts.Interfaces;

namespace Peasie.Contracts
{

    public class PeasieRequestDTO : IToHtmlTable
    {
        public Version Version { get; set; } = new Version(1, 0, 0, 0);
        public string? Id { get; set; }
        public string? Payload { get; set; }
    }
}
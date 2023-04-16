using Peasie.Contracts;

namespace PeasieLib.Handlers;

internal class SessionWrapper
{
    public SessionRequestDTO? SessionRequest { get; set; }
    public SessionResponseDTO? SessionResponse { get; set; }
    public SessionDetailsDTO? SessionDetails { get; set; }
    public string? PrivateKey { get; set; }
}

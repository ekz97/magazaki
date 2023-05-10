using Peasie.Contracts;

namespace PeasieLib.Handlers;

public class SessionWrapper
{
    public SessionRequestDTO? SessionRequest { get; set; }
    public SessionResponseDTO? SessionResponse { get; set; }
    public SessionDetailsDTO? SessionDetails { get; set; }
    public string? PrivateKey { get; set; }
    public bool Valid { get; set; } = true;
}

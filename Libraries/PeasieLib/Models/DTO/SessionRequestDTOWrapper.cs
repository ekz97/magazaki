using Peasie.Contracts;
using System.Security.Cryptography;

namespace PeasieLib.Models.DTO
{
    public class SessionRequestDTOWrapper
    {
        public SessionRequestDTO? SessionRequest { get; set; }
        public SessionResponseDTO? SessionResponse { get; set; }
        public SessionDetailsDTO? SessionDetails { get; set; }
        public RSA? RSA { get; set; }      
        public string? PrivateKey { get; set; }
        public string? PublicKey { get; set; }        
    }
}

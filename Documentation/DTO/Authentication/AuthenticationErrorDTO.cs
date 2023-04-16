using System.Text.Json.Serialization;

namespace PeasieLib.DTO.Authentication
{
    // AuthenticationErrorDTO myDeserializedClass = JsonSerializer.Deserialize<AuthenticationErrorDTO>(myJsonResponse);
    public class AuthenticationErrorDTO
    {
        [JsonConstructor]
        public AuthenticationErrorDTO(
            string errorCode,
            string message
        )
        {
            ErrorCode = errorCode;
            Message = message;
        }

        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; }

        [JsonPropertyName("message")]
        public string Message { get; }
    }


}

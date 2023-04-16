using System.Text.Json.Serialization;

namespace PeasieLib.DTO.Authentication
{
    public class AccountHoldersDTO
    {
        [JsonConstructor]
        public AccountHoldersDTO(
            AccountHolderDTO authenticationInfo
        )
        {
            AuthenticationInfo = authenticationInfo;
        }

        [JsonPropertyName("AuthenticationInfo")]
        public AccountHolderDTO AuthenticationInfo { get; }
    }


}

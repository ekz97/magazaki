using System.Text.Json.Serialization;

namespace PeasieLib.DTO.Authentication
{
    public class AccountHolderDTO
    {
        [JsonConstructor]
        public AccountHolderDTO(
            string accountHolderDisplayName,
            string id,
            string accountHolderType,
            string accountHolderStatus,
            string referralId
        )
        {
            AccountHolderDisplayName = accountHolderDisplayName;
            Id = id;
            AccountHolderType = accountHolderType;
            AccountHolderStatus = accountHolderStatus;
            ReferralId = referralId;
        }

        [JsonPropertyName("accountHolderDisplayName")]
        public string AccountHolderDisplayName { get; }

        [JsonPropertyName("id")]
        public string Id { get; }

        [JsonPropertyName("accountHolderType")]
        public string AccountHolderType { get; }

        [JsonPropertyName("accountHolderStatus")]
        public string AccountHolderStatus { get; }

        [JsonPropertyName("referralId")]
        public string ReferralId { get; }
    }


}

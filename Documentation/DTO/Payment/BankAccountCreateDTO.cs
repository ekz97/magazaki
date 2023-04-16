using Peasie.Contracts;
using System.Text.Json.Serialization;

namespace PeasieLib.DTO.Payment
{
    public class BankAccountCreateDTO
    {
        [JsonConstructor]
        public BankAccountCreateDTO(
            string id,
            DateTime? createdOn,
            string status,
            AmountDTO actualBalance,
            AmountDTO availableBalance,
            string internalAccountId,
            string transactionCategory,
            string accountType,
            string friendlyName
        )
        {
            Id = id;
            CreatedOn = createdOn;
            Status = status; // PENDING, ACTIVE, FAILED, SUSPENDED, CLOSED
            ActualBalance = actualBalance;
            AvailableBalance = availableBalance;
            InternalAccountId = internalAccountId;
            TransactionCategory = transactionCategory;
            AccountType = accountType;
            FriendlyName = friendlyName;
        }

        [JsonPropertyName("id")]
        public string Id { get; }

        [JsonPropertyName("createdOn")]
        public DateTime? CreatedOn { get; }

        [JsonPropertyName("status")]
        public string Status { get; }

        [JsonPropertyName("actualBalance")]
        public AmountDTO ActualBalance { get; }

        [JsonPropertyName("availableBalance")]
        public AmountDTO AvailableBalance { get; }

        [JsonPropertyName("internalAccountId")]
        public string InternalAccountId { get; }

        [JsonPropertyName("transactionCategory")]
        public string TransactionCategory { get; }

        [JsonPropertyName("accountType")]
        public string AccountType { get; }

        [JsonPropertyName("friendlyName")]
        public string FriendlyName { get; }
    }
}

using Peasie.Contracts;
using System.Text.Json.Serialization;

namespace PeasieLib.DTO.Payment
{
    // AccountDetailDTO myDeserializedClass = JsonSerializer.Deserialize<AccountDetailDTO>(myJsonResponse);
    public class AccountDetailDTO
    {
        [JsonConstructor]
        public AccountDetailDTO(
            string id,
            DateTime? createdOn,
            string status,
            AmountDTO actualBalance,
            AmountDTO availableBalance,
            string transactionCategory,
            string accountType,
            string friendlyName,
            string internalAccountId
        )
        {
            this.id = id;
            this.createdOn = createdOn;
            this.status = status;
            this.actualBalance = actualBalance;
            this.availableBalance = availableBalance;
            this.transactionCategory = transactionCategory;
            this.accountType = accountType;
            this.friendlyName = friendlyName;
            this.internalAccountId = internalAccountId;
        }

        [JsonPropertyName("id")]
        public string id { get; }

        [JsonPropertyName("createdOn")]
        public DateTime? createdOn { get; }

        [JsonPropertyName("status")]
        public string status { get; }

        [JsonPropertyName("actualBalance")]
        public AmountDTO actualBalance { get; }

        [JsonPropertyName("availableBalance")]
        public AmountDTO availableBalance { get; }

        [JsonPropertyName("transactionCategory")]
        public string transactionCategory { get; }

        [JsonPropertyName("accountType")]
        public string accountType { get; }

        [JsonPropertyName("friendlyName")]
        public string friendlyName { get; }

        [JsonPropertyName("internalAccountId")]
        public string internalAccountId { get; }
    }

    /*
Transaction Types

The transaction Status webhook can be setup for the following transaction types:

PAYIN - Payins received into an OpenPayd account
PAYOUT - Payouts made from an OpenPayd account to an external bank account
TRANSFER - Transfers between two OpenPayd accounts
EXCHANGE - Exchange between two OpenPayd accounts in different currencies
RETURN_IN - Returned Payouts due to being rejected by the beneficiary bank
RETURN_OUT - Payins returned to sender from your account as a result of a Recall from the sender bank
FEE - Fees applied as a separate transaction instead of being deducted in line from another transaction
Direct Debit - Direct Debits incoming/outgoing for an OpenPayd account

Transaction Statuses

Events can be setup for one or multiple statuses from the following:

PROCESSING - The transaction is being processed by OpenPayd and regulatory checks are being performed before it can be accepted from or submitted to the banking provider.
RELEASED - The transaction has been successfully submitted to the banking / FX provider for processing.
COMPLETED - The transaction has been successfully processed by OpenPayd (PAYIN) or the banking provider (PAYOUT, EXCHANGE).
FAILED - The transaction was failed by OpenPayd or the banking provider.
CANCELLED - The transaction was cancelled by the user before it was completed.
    */
    // DoS attack: prevent
}

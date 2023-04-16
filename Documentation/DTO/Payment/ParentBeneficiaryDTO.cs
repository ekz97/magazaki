using System.Text.Json.Serialization;

namespace PeasieLib.DTO.Payment
{
    // ParentBeneficiaryDTO myDeserializedClass = JsonSerializer.Deserialize<ParentBeneficiaryDTO>(myJsonResponse);
    public class ParentBeneficiaryDTO
    {
        [JsonConstructor]
        public ParentBeneficiaryDTO(
            string beneficiaryType,
            string id,
            string accountHolderId,
            string title,
            string firstName,
            string lastName,
            string friendlyName,
            object tag
        )
        {
            this.beneficiaryType = beneficiaryType;
            this.id = id;
            this.accountHolderId = accountHolderId;
            this.title = title;
            this.firstName = firstName;
            this.lastName = lastName;
            this.friendlyName = friendlyName;
            this.tag = tag;
        }

        [JsonPropertyName("beneficiaryType")]
        public string beneficiaryType { get; }

        [JsonPropertyName("id")]
        public string id { get; }

        [JsonPropertyName("accountHolderId")]
        public string accountHolderId { get; }

        [JsonPropertyName("title")]
        public string title { get; }

        [JsonPropertyName("firstName")]
        public string firstName { get; }

        [JsonPropertyName("lastName")]
        public string lastName { get; }

        [JsonPropertyName("friendlyName")]
        public string friendlyName { get; }

        [JsonPropertyName("tag")]
        public object tag { get; }
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

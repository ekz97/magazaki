using Peasie.Contracts;
using System.Text.Json.Serialization;

namespace PeasieLib.DTO.Payment
{
    // TransactionDetailsDTO myDeserializedClass = JsonSerializer.Deserialize<TransactionDetailsDTO>(myJsonResponse);
    public class TransactionDetailsDTO
    {
        [JsonConstructor]
        public TransactionDetailsDTO(
            string id,
            string shortId,
            string accountId,
            DateTime? createdDate,
            DateTime? updatedDate,
            DateTime? completedDate,
            string transactionId,
            string transactionCategory,
            string type,
            SourceInfoDTO sourceInfo,
            DestinationInfoDTO destinationInfo,
            object source,
            string destination,
            AmountDTO totalAmount,
            AmountDTO amount,
            AmountDTO fee,
            AmountDTO runningBalance,
            object fxRate,
            string status,
            object failureReason,
            string comment,
            object transactionReference,
            object referenceAmount,
            string accountHolderId
        )
        {
            this.id = id;
            this.shortId = shortId;
            this.accountId = accountId;
            this.createdDate = createdDate;
            this.updatedDate = updatedDate;
            this.completedDate = completedDate;
            this.transactionId = transactionId;
            this.transactionCategory = transactionCategory;
            this.type = type;
            this.sourceInfo = sourceInfo;
            this.destinationInfo = destinationInfo;
            this.source = source;
            this.destination = destination;
            this.totalAmount = totalAmount;
            this.amount = amount;
            this.fee = fee;
            this.runningBalance = runningBalance;
            this.fxRate = fxRate;
            this.status = status;
            this.failureReason = failureReason;
            this.comment = comment;
            this.transactionReference = transactionReference;
            this.referenceAmount = referenceAmount;
            this.accountHolderId = accountHolderId;
        }

        [JsonPropertyName("id")]
        public string id { get; }

        [JsonPropertyName("shortId")]
        public string shortId { get; }

        [JsonPropertyName("accountId")]
        public string accountId { get; }

        [JsonPropertyName("createdDate")]
        public DateTime? createdDate { get; }

        [JsonPropertyName("updatedDate")]
        public DateTime? updatedDate { get; }

        [JsonPropertyName("completedDate")]
        public DateTime? completedDate { get; }

        [JsonPropertyName("transactionId")]
        public string transactionId { get; }

        [JsonPropertyName("transactionCategory")]
        public string transactionCategory { get; }

        [JsonPropertyName("type")]
        public string type { get; }

        [JsonPropertyName("sourceInfo")]
        public SourceInfoDTO sourceInfo { get; }

        [JsonPropertyName("destinationInfo")]
        public DestinationInfoDTO destinationInfo { get; }

        [JsonPropertyName("source")]
        public object source { get; }

        [JsonPropertyName("destination")]
        public string destination { get; }

        [JsonPropertyName("totalAmount")]
        public AmountDTO totalAmount { get; }

        [JsonPropertyName("amount")]
        public AmountDTO amount { get; }

        [JsonPropertyName("fee")]
        public AmountDTO fee { get; }

        [JsonPropertyName("runningBalance")]
        public AmountDTO runningBalance { get; }

        [JsonPropertyName("fxRate")]
        public object fxRate { get; }

        [JsonPropertyName("status")]
        public string status { get; }

        [JsonPropertyName("failureReason")]
        public object failureReason { get; }

        [JsonPropertyName("comment")]
        public string comment { get; }

        [JsonPropertyName("transactionReference")]
        public object transactionReference { get; }

        [JsonPropertyName("referenceAmount")]
        public object referenceAmount { get; }

        [JsonPropertyName("accountHolderId")]
        public string accountHolderId { get; }
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

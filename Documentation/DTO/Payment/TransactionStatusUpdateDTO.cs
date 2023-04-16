using Peasie.Contracts;
using System.Text.Json.Serialization;

namespace PeasieLib.DTO.Payment
{
    // WebHooks: reverse api

    // 1. Pay In Completed
    // 2. Pay In Processing
    // 3. Pay In Failed
    // 4. Payout Completed
    // 5. Payout Failed
    // 6. Payout Cancelled
    // 7. Transfer Completed
    // 8. Transfer Failed
    // 9. Transfer Cancelled
    // 10. Exchange Completed
    // 11. Exchange Failed
    // 12. Return In Completed
    // 13. Return In Failed
    // 14. Return Out Completed
    // 15. FeeDTO Completed
    // 16. Account Status Updated


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

    public class TransactionStatusUpdateDTO
    {
        [JsonConstructor]
        public TransactionStatusUpdateDTO(
            string type,
            string id,
            string shortId,
            string accountId,
            DateTime? createdDate,
            DateTime? updatedDate,
            string transactionId,
            string transactionCategory,
            string paymentType,
            string status,
            string accountHolderId,
            SourceInfoDTO sourceInfo,
            DestinationInfoDTO destinationInfo,
            object source,
            string destination,
            object fxRate,
            object midMarketRate,
            object fixedSide,
            AmountDTO totalAmount,
            AmountDTO amount,
            object buyAmount,
            AmountDTO fee,
            AmountDTO runningBalance,
            object failureReason,
            string comment,
            object transactionReference,
            object referenceAmount,
            string createdBy,
            object checkedBy,
            object checkedDate,
            object mandateId,
            object originalTransactionId
        )
        {
            this.type = type;
            this.id = id;
            this.shortId = shortId;
            this.accountId = accountId;
            this.createdDate = createdDate;
            this.updatedDate = updatedDate;
            this.transactionId = transactionId;
            this.transactionCategory = transactionCategory;
            this.paymentType = paymentType;
            this.status = status;
            this.accountHolderId = accountHolderId;
            this.sourceInfo = sourceInfo;
            this.destinationInfo = destinationInfo;
            this.source = source;
            this.destination = destination;
            this.fxRate = fxRate;
            this.midMarketRate = midMarketRate;
            this.fixedSide = fixedSide;
            this.totalAmount = totalAmount;
            this.amount = amount;
            this.buyAmount = buyAmount;
            this.fee = fee;
            this.runningBalance = runningBalance;
            this.failureReason = failureReason;
            this.comment = comment;
            this.transactionReference = transactionReference;
            this.referenceAmount = referenceAmount;
            this.createdBy = createdBy;
            this.checkedBy = checkedBy;
            this.checkedDate = checkedDate;
            this.mandateId = mandateId;
            this.originalTransactionId = originalTransactionId;
        }

        [JsonPropertyName("type")]
        public string type { get; }

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

        [JsonPropertyName("transactionId")]
        public string transactionId { get; }

        [JsonPropertyName("transactionCategory")]
        public string transactionCategory { get; }

        [JsonPropertyName("paymentType")]
        public string paymentType { get; }

        [JsonPropertyName("status")]
        public string status { get; }

        [JsonPropertyName("accountHolderId")]
        public string accountHolderId { get; }

        [JsonPropertyName("sourceInfo")]
        public SourceInfoDTO sourceInfo { get; }

        [JsonPropertyName("destinationInfo")]
        public DestinationInfoDTO destinationInfo { get; }

        [JsonPropertyName("source")]
        public object source { get; }

        [JsonPropertyName("destination")]
        public string destination { get; }

        [JsonPropertyName("fxRate")]
        public object fxRate { get; }

        [JsonPropertyName("midMarketRate")]
        public object midMarketRate { get; }

        [JsonPropertyName("fixedSide")]
        public object fixedSide { get; }

        [JsonPropertyName("totalAmount")]
        public AmountDTO totalAmount { get; }

        [JsonPropertyName("amount")]
        public AmountDTO amount { get; }

        [JsonPropertyName("buyAmount")]
        public object buyAmount { get; }

        [JsonPropertyName("fee")]
        public AmountDTO fee { get; }

        [JsonPropertyName("runningBalance")]
        public AmountDTO runningBalance { get; }

        [JsonPropertyName("failureReason")]
        public object failureReason { get; }

        [JsonPropertyName("comment")]
        public string comment { get; }

        [JsonPropertyName("transactionReference")]
        public object transactionReference { get; }

        [JsonPropertyName("referenceAmount")]
        public object referenceAmount { get; }

        [JsonPropertyName("createdBy")]
        public string createdBy { get; }

        [JsonPropertyName("checkedBy")]
        public object checkedBy { get; }

        [JsonPropertyName("checkedDate")]
        public object checkedDate { get; }

        [JsonPropertyName("mandateId")]
        public object mandateId { get; }

        [JsonPropertyName("originalTransactionId")]
        public object originalTransactionId { get; }
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

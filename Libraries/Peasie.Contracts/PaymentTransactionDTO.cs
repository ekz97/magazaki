using System.Text.Json.Serialization;
using System.Transactions;
using System.Xml.Linq;
using Peasie.Contracts.Interfaces;

namespace Peasie.Contracts
{

    public class PaymentTransactionDTO : IToHtmlTable
    {
        [JsonConstructor]
        public PaymentTransactionDTO(
            string id,
            string shortId,
            string accountId,
            DateTime? createdDate,
            DateTime? updatedDate,
            DateTime? paymentDate,
            string transactionId,
            string transactionCategory,
            string paymentType,
            string type,
            SourceInfoDTO sourceInfo,
            DestinationInfoDTO destinationInfo,
            object source,
            string destination,
            AmountDTO totalAmount,
            AmountDTO amount,
            AmountDTO fee,
            AmountDTO runningBalance,
            object buyAmount,
            object fxRate,
            object midMarketRate,
            object fixedSide,
            string status,
            object failureReason,
            string comment,
            object transactionReference,
            object referenceAmount,
            string accountHolderId
        )
        {
            Id = id;
            ShortId = shortId;
            AccountId = accountId;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            PaymentDate = paymentDate;
            TransactionId = transactionId;
            TransactionCategory = transactionCategory;
            PaymentType = paymentType;
            Type = type;
            SourceInfo = sourceInfo;
            DestinationInfo = destinationInfo;
            Source = source;
            Destination = destination;
            TotalAmount = totalAmount;
            Amount = amount;
            Fee = fee;
            RunningBalance = runningBalance;
            BuyAmount = buyAmount;
            FxRate = fxRate;
            MidMarketRate = midMarketRate;
            FixedSide = fixedSide;
            Status = status;
            FailureReason = failureReason;
            Comment = comment;
            TransactionReference = transactionReference;
            ReferenceAmount = referenceAmount;
            AccountHolderId = accountHolderId;
        }

        [JsonPropertyName("id")]
        public string? Id { get; }

        [JsonPropertyName("shortId")]
        public string? ShortId { get; }

        [JsonPropertyName("accountId")]
        public string? AccountId { get; }

        [JsonPropertyName("createdDate")]
        public DateTime? CreatedDate { get; }

        [JsonPropertyName("updatedDate")]
        public DateTime? UpdatedDate { get; }

        [JsonPropertyName("paymentDate")]
        public DateTime? PaymentDate { get; }

        [JsonPropertyName("transactionId")]
        public string TransactionId { get; }

        [JsonPropertyName("transactionCategory")]
        public string TransactionCategory { get; }

        [JsonPropertyName("paymentType")]
        public string? PaymentType { get; }

        [JsonPropertyName("type")]
        public string? Type { get; }

        [JsonPropertyName("sourceInfo")]
        public SourceInfoDTO? SourceInfo { get; }

        [JsonPropertyName("destinationInfo")]
        public DestinationInfoDTO? DestinationInfo { get; }

        [JsonPropertyName("source")]
        public object? Source { get; }

        [JsonPropertyName("destination")]
        public string? Destination { get; }

        [JsonPropertyName("totalAmount")]
        public AmountDTO? TotalAmount { get; }

        [JsonPropertyName("amount")]
        public AmountDTO? Amount { get; }

        [JsonPropertyName("fee")]
        public AmountDTO Fee { get; }

        [JsonPropertyName("runningBalance")]
        public AmountDTO? RunningBalance { get; }

        [JsonPropertyName("buyAmount")]
        public object? BuyAmount { get; }

        [JsonPropertyName("fxRate")]
        public object FxRate { get; }

        [JsonPropertyName("midMarketRate")]
        public object? MidMarketRate { get; }

        [JsonPropertyName("fixedSide")]
        public object? FixedSide { get; }

        [JsonPropertyName("status")]
        public string? Status { get; set; } // Status: shows transaction as INITIATED; possible values are INITIATED, PROCESSING, RELEASED, COMPLETED, FAILED

        [JsonPropertyName("failureReason")]
        public object? FailureReason { get; }

        [JsonPropertyName("comment")]
        public string? Comment { get; }

        [JsonPropertyName("transactionReference")]
        public object? TransactionReference { get; }

        [JsonPropertyName("referenceAmount")]
        public object? ReferenceAmount { get; }

        [JsonPropertyName("accountHolderId")]
        public string? AccountHolderId { get; }

        /*
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        */

        public override string? ToString()
        {
            return "Id: " + Id +
                   ", ShortId: " + ShortId +
                   ", AccountId: " + AccountId +
                   ", CreatedDate: " + CreatedDate +
                   ", UpdatedDate: " + UpdatedDate +
                   ", PaymentDate: " + PaymentDate +
                   ", TransactionId: " + TransactionId +
                   ", TransactionCategory: " + TransactionCategory +
                   ", PaymentType: " + PaymentType +
                   ", Type: " + Type +
                   ", SourceInfo: " + SourceInfo +
                   ", DestinationInfo: " + DestinationInfo +
                   ", Source: " + Source +
                   ", Destination: " + Destination +
                   ", TotalAmount: " + TotalAmount +
                   ", Amount: " + Amount +
                   ", Fee: " + Fee +
                   ", RunningBalance: " + RunningBalance +
                   ", BuyAmount: " + BuyAmount +
                   ", FxRate: " + FxRate +
                   ", MidMarketRate: " + MidMarketRate +
                   ", FixedSide: " + FixedSide +
                   ", Status: " + Status +
                   ", FailureReason: " + FailureReason +
                   ", Comment: " + Comment +
                   ", TransactionReference: " + TransactionReference +
                   ", ReferenceAmount: " + ReferenceAmount +
                   ", AccountHolderId: " + AccountHolderId;                    
        }
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

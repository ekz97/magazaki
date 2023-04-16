using System.Text.Json.Serialization;

namespace PeasieAPI.DTO.Paging
{
    public class PageableDTO
    {
        [JsonConstructor]
        public PageableDTO(
            SortDTO sort,
            int? pageSize,
            int? pageNumber,
            int? offset,
            bool? paged,
            bool? unpaged
        )
        {
            this.sort = sort;
            this.pageSize = pageSize;
            this.pageNumber = pageNumber;
            this.offset = offset;
            this.paged = paged;
            this.unpaged = unpaged;
        }

        [JsonPropertyName("sort")]
        public SortDTO sort { get; }

        [JsonPropertyName("pageSize")]
        public int? pageSize { get; }

        [JsonPropertyName("pageNumber")]
        public int? pageNumber { get; }

        [JsonPropertyName("offset")]
        public int? offset { get; }

        [JsonPropertyName("paged")]
        public bool? paged { get; }

        [JsonPropertyName("unpaged")]
        public bool? unpaged { get; }
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

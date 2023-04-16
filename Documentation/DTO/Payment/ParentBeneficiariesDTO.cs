using PeasieAPI.DTO.Paging;
using System.Text.Json.Serialization;

namespace PeasieLib.DTO.Payment
{
    // ParentBeneficiariesDTO myDeserializedClass = JsonSerializer.Deserialize<ParentBeneficiariesDTO>(myJsonResponse);
    public class ParentBeneficiariesDTO
    {
        [JsonConstructor]
        public ParentBeneficiariesDTO(
            List<ContentDTO> content,
            PageableDTO pageable,
            int? totalPages,
            int? totalElements,
            bool? last,
            bool? first,
            SortDTO sort,
            int? numberOfElements,
            int? size,
            int? number,
            bool? empty
        )
        {
            this.content = content;
            this.pageable = pageable;
            this.totalPages = totalPages;
            this.totalElements = totalElements;
            this.last = last;
            this.first = first;
            this.sort = sort;
            this.numberOfElements = numberOfElements;
            this.size = size;
            this.number = number;
            this.empty = empty;
        }

        [JsonPropertyName("content")]
        public IReadOnlyList<ContentDTO> content { get; }

        [JsonPropertyName("pageable")]
        public PageableDTO pageable { get; }

        [JsonPropertyName("totalPages")]
        public int? totalPages { get; }

        [JsonPropertyName("totalElements")]
        public int? totalElements { get; }

        [JsonPropertyName("last")]
        public bool? last { get; }

        [JsonPropertyName("first")]
        public bool? first { get; }

        [JsonPropertyName("sort")]
        public SortDTO sort { get; }

        [JsonPropertyName("numberOfElements")]
        public int? numberOfElements { get; }

        [JsonPropertyName("size")]
        public int? size { get; }

        [JsonPropertyName("number")]
        public int? number { get; }

        [JsonPropertyName("empty")]
        public bool? empty { get; }
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

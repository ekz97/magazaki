using System.Text.Json.Serialization;

namespace PeasieLib.DTO.Payment
{
    public class BeneficiaryContentDTO
    {
        [JsonConstructor]
        public BeneficiaryContentDTO(
            string id,
            string accountHolderId,
            string beneficiaryId,
            string externalId,
            string status,
            string beneficiaryFirstName,
            string beneficiaryLastName,
            string beneficiaryType,
            string beneficiaryCountry,
            string beneficiaryPostalCode,
            string beneficiaryCity,
            string accountNumber,
            string iban,
            string bic,
            string currency,
            string bankName,
            string bankAddress,
            string bankAccountCountry,
            string bankAccountHolderName,
            string companyName,
            List<string> paymentTypes,
            List<BankRoutingCodeDTO> bankRoutingCodes,
            string cls,
            string bankAccountType
        )
        {
            this.id = id;
            this.accountHolderId = accountHolderId;
            this.beneficiaryId = beneficiaryId;
            this.externalId = externalId;
            this.status = status;
            this.beneficiaryFirstName = beneficiaryFirstName;
            this.beneficiaryLastName = beneficiaryLastName;
            this.beneficiaryType = beneficiaryType;
            this.beneficiaryCountry = beneficiaryCountry;
            this.beneficiaryPostalCode = beneficiaryPostalCode;
            this.beneficiaryCity = beneficiaryCity;
            this.accountNumber = accountNumber;
            this.iban = iban;
            this.bic = bic;
            this.currency = currency;
            this.bankName = bankName;
            this.bankAddress = bankAddress;
            this.bankAccountCountry = bankAccountCountry;
            this.bankAccountHolderName = bankAccountHolderName;
            this.companyName = companyName;
            this.paymentTypes = paymentTypes;
            this.bankRoutingCodes = bankRoutingCodes;
            this.cls = cls;
            this.bankAccountType = bankAccountType;
        }

    [JsonPropertyName("id")]
    public string id { get; }

    [JsonPropertyName("accountHolderId")]
    public string accountHolderId { get; }

    [JsonPropertyName("beneficiaryId")]
    public string beneficiaryId { get; }

    [JsonPropertyName("externalId")]
    public string externalId { get; }

    [JsonPropertyName("status")]
    public string status { get; }

    [JsonPropertyName("beneficiaryFirstName")]
    public string beneficiaryFirstName { get; }

    [JsonPropertyName("beneficiaryLastName")]
    public string beneficiaryLastName { get; }

    [JsonPropertyName("beneficiaryType")]
    public string beneficiaryType { get; }

    [JsonPropertyName("beneficiaryCountry")]
    public string beneficiaryCountry { get; }

    [JsonPropertyName("beneficiaryPostalCode")]
    public string beneficiaryPostalCode { get; }

    [JsonPropertyName("beneficiaryCity")]
    public string beneficiaryCity { get; }

    [JsonPropertyName("accountNumber")]
    public string accountNumber { get; }

    [JsonPropertyName("iban")]
    public string iban { get; }

    [JsonPropertyName("bic")]
    public string bic { get; }

    [JsonPropertyName("currency")]
    public string currency { get; }

    [JsonPropertyName("bankName")]
    public string bankName { get; }

    [JsonPropertyName("bankAddress")]
    public string bankAddress { get; }

    [JsonPropertyName("bankAccountCountry")]
    public string bankAccountCountry { get; }

    [JsonPropertyName("bankAccountHolderName")]
    public string bankAccountHolderName { get; }

    [JsonPropertyName("companyName")]
    public string companyName { get; }

    [JsonPropertyName("paymentTypes")]
    public IReadOnlyList<string> paymentTypes { get; }

    [JsonPropertyName("bankRoutingCodes")]
    public IReadOnlyList<BankRoutingCodeDTO> bankRoutingCodes { get; }

    [JsonPropertyName("class")]
    public string cls { get; }

    [JsonPropertyName("bankAccountType")]
    public string bankAccountType { get; }
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

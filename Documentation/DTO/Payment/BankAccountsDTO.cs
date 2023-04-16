using System.Text.Json.Serialization;

namespace PeasieLib.DTO.Payment
{
    // BankAccountsDTO myDeserializedClass = JsonSerializer.Deserialize<List<BankAccountsDTO>>(myJsonResponse);
    public class BankAccountsDTO
    {
        [JsonConstructor]
        public BankAccountsDTO(
            string currency,
            string status,
            string internalAccountId,
            string bankCountry,
            string bankAddress,
            string swiftCode,
            string iban,
            object accountNumber,
            string bankName,
            string bankAccountHolderName,
            List<object> routingCodeEntries
        )
        {
            this.currency = currency;
            this.status = status;
            this.internalAccountId = internalAccountId;
            this.bankCountry = bankCountry;
            this.bankAddress = bankAddress;
            this.swiftCode = swiftCode;
            this.iban = iban;
            this.accountNumber = accountNumber;
            this.bankName = bankName;
            this.bankAccountHolderName = bankAccountHolderName;
            this.routingCodeEntries = routingCodeEntries;
        }

        [JsonPropertyName("currency")]
        public string currency { get; }

        [JsonPropertyName("status")]
        public string status { get; }

        [JsonPropertyName("internalAccountId")]
        public string internalAccountId { get; }

        [JsonPropertyName("bankCountry")]
        public string bankCountry { get; }

        [JsonPropertyName("bankAddress")]
        public string bankAddress { get; }

        [JsonPropertyName("swiftCode")]
        public string swiftCode { get; }

        [JsonPropertyName("iban")]
        public string iban { get; }

        [JsonPropertyName("accountNumber")]
        public object accountNumber { get; }

        [JsonPropertyName("bankName")]
        public string bankName { get; }

        [JsonPropertyName("bankAccountHolderName")]
        public string bankAccountHolderName { get; }

        [JsonPropertyName("routingCodeEntries")]
        public IReadOnlyList<object> routingCodeEntries { get; }
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

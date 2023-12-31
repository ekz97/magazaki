﻿using System.ComponentModel.DataAnnotations;

namespace BeneficiaryAPI.Handlers
{
    public class PaymentTrxDTO
    {
        [Required]
        [MaxLength(16)]
        string? IBAN { get; set; }

        [Required]
        public string? TransactionId { get; set; }

        [Required]
        public decimal? Amount { get; set; }

        [Required]
        private string _currency = "EUR";
        public string? Currency { get { return _currency; } set {
                _currency = !string.IsNullOrWhiteSpace(value) &&
                       (value == "EUR" ||
                       value == "USD")
                ? value
                : throw new ArgumentException("Currency value was not allowed. Value: " + value);
            }
        }

        public string? Comment { get; set; }
    }
}
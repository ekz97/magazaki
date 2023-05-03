using System.ComponentModel.DataAnnotations;
using Peasie.Contracts.Interfaces;

namespace Peasie.Contracts
{
    public class UserDTO : IToHtmlTable
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Type { get; set; } // BANK or SHOP
        [Required]
        public string Designation { get; set; } // BANK or SHOP NAME
    }
}
using System.ComponentModel.DataAnnotations;

namespace Peasie.Contracts
{
    public class UserDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Type { get; set; } // BANK or SHOP
        [Required]
        public string Designation { get; set; } // BANK or SHOP NAME
    }
}
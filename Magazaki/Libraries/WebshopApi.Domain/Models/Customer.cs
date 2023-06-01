namespace WebshopApi.Domain.Models
{
    public class Customer
    {

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}

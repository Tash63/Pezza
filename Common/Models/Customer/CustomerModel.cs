namespace Common.Models.Customer
{
    public class CustomerModel
    {
        public int Id { get; set; }

        required public string Name { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Cellphone { get; set; }

        public DateTime DateCreated { get; set; }
    }
}

namespace Common.Models.Customer
{
    public class SearchCustomerModel
    {
        public string? OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; } = PagingArgs.NoPaging;
        public int Id { get; set; }

        public required string? Name { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Cellphone { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}



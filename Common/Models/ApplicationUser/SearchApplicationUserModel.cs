namespace Common.Models.ApplicationUser
{
    public class SearchApplicationUserModel
    {
        public string? OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; } = PagingArgs.NoPaging;

        public required string? FullName { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Cellphone { get; set; }

        public DateOnly? DateCreated { get; set; }
    }
}

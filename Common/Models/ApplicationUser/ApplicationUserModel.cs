using System.Security.Claims;

namespace Common.Models.ApplicationUser
{
    public class ApplicationUserModel
    {
        public string? Email { get; set; }

        public string? Adress { get; set; }

        public string? PhoneNumber { get; set; }

        public DateOnly? DateCreated { get; set; }

        public string? FullName { get; set; }

        public IList<Claim> userClaim { get; set; }
    }
}

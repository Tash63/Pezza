using Microsoft.AspNetCore.Identity;

namespace Common.Entities
{
    public sealed class ApplicationUser:IdentityUser
    {
        public string? FullName { get; set; }
        public string? Adress { get; set; }
        public DateOnly? DateCreated { get; set; }

    }
}

namespace Common.Models.ApplicationUser
{
    public class UpdateApplicationUserModel
    {
        public string? Adress { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly? CreatedDate { get; set; }
        public string? Fullname { get; set; }
    }
}

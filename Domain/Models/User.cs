namespace Domain.Models
{
    public class User : BaseModel
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public HashSet<UserRole> UserRole { get; set; } = new HashSet<UserRole>();
    }
}
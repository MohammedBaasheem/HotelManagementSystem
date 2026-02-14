namespace Domain.Models
{
    public class User : BaseModel
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        private string _email;
        public string Email
        {
            get => _email;
            set => _email = value?.Trim().ToLower();
        }
        public string PasswordHash { get; set; }
        public HashSet<UserRole> UserRole { get; set; } = new HashSet<UserRole>();
    }
}
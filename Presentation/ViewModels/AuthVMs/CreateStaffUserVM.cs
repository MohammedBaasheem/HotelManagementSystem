namespace Presentation.ViewModels.AuthVMs
{
    public class CreateStaffUserVM
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
        public string Country { get; set; }

        public Guid RoleId { get; set; }
    }
}

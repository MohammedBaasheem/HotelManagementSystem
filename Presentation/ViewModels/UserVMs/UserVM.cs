namespace Presentation.ViewModels.UserVMs
{
    public class UserVM
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }

    }
}

namespace Presentation.ViewModels.UserVMs
{
    public class UserWithRolesVM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public IEnumerable<Guid> RoleIds { get; set; }

    }
}

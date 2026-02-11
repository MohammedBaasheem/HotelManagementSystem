namespace Application.DTOs.UserDTOs
{
    public class UserWithRolesDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public IEnumerable<Guid> RoleIds { get; set; }

    }
}

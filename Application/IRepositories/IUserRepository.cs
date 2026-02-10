using Domain.Models;

namespace Application.IRepositories
{
    public interface IUserRepository : IGeneralRepository<User>
    {
        public Task AddUserRoleAsync(UserRole userRole);
    }
}

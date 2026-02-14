using Domain.Models;

namespace Application.IRepositories
{
    public interface IUserRepository : IGeneralRepository<User>
    {
        public Task AddUserRoleAsync(UserRole userRole);
        Task<bool> IsUserInRoleAsync(Guid userId, Guid roleId);
        public Task<IEnumerable<Guid>> GetRoleIdsAsync(Guid userId);
    }
}

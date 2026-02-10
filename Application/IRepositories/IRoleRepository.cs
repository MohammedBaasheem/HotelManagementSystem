using Domain.Models;

namespace Application.IRepositories
{
    public interface IRoleRepository : IGeneralRepository<Role>
    {
        public Task<Guid> GetRoleIdByNameAsync(string roleName);
    }
}

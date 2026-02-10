using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoleRepository : GeneralRepository<Role>, IRoleRepository
    {
        public RoleRepository(Context context) : base(context)
        {
        }
        public async Task<Guid> GetRoleIdByNameAsync(string roleName)
        {
            return await GetAll().Where(R => R.Name == roleName.ToUpperInvariant()).Select(r => r.Id).FirstOrDefaultAsync();
        }
    }
}

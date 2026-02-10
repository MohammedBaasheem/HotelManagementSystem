using Application.IRepositories;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class UserRepository : GeneralRepository<User>, IUserRepository
    {
        public UserRepository(Context context) : base(context)
        {
        }

        public async Task AddUserRoleAsync(UserRole userRole)
        {

            await _context.UserRoles.AddAsync(userRole);
        }
    }
}

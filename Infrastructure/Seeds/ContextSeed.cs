using Application.Helpers;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Seeds
{
    public class ContextSeed
    {
        public static async Task SeedAsync(Context context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
            {
                new Role { Name = Roles.ADMIN.ToString(), CreatedAt = DateTime.UtcNow }, 
                new Role { Name = Roles.CUSTOMER.ToString(), CreatedAt = DateTime.UtcNow }
            };

                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();

            }

            if (!context.Users.Any(u => u.Email == "admin@hotel.com"))
            {
                var adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@hotel.com",
                    PhoneNumber = "",
                    Country = "",
                    PasswordHash = PasswordHasher.HashPassword("admin123"),
                    CreatedAt = DateTime.UtcNow
                };

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();


                var adminRole = context.Roles.FirstOrDefault(r => r.Name == Roles.ADMIN.ToString());

                if (adminRole != null)
                {
                    var userRole = new UserRole
                    {
                        UserId = adminUser.Id,
                        RoleId = adminRole.Id,
                        CreatedAt= DateTime.UtcNow
                    };

                    await context.UserRoles.AddAsync(userRole);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

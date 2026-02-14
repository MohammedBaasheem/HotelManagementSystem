using Application.Interface;
using Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Helpers
{
    public static class ApplicationDependencies
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserManagementService, UserManagmentService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddAutoMapper(typeof(ApplicationDependencies).Assembly);
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}

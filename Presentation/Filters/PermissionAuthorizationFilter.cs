using Application.Interface;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Presentation.Filters
{
    public class PermissionAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly Feature _feature;
        private readonly IRoleService _roleService;

        public PermissionAuthorizationFilter(Feature feature, IRoleService roleService)
        {
            _feature = feature;
            _roleService = roleService;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var userRoleIds = new List<Guid>();

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var roleClaims = user.FindAll(ClaimTypes.Role);

                foreach (var claim in roleClaims)
                {
                    if (Guid.TryParse(claim.Value, out var guidValue))
                    {
                        userRoleIds.Add(guidValue);
                    }
                }
            }
            else
            {
                var guestId = await _roleService.GetRoleIdByNameAsync(Roles.GUEST.ToString());
                userRoleIds.Add(guestId);
            }

            var adminRoleId = await _roleService.GetRoleIdByNameAsync(Roles.ADMIN.ToString());

            if (userRoleIds.Contains(adminRoleId))
            {
                return;
            }
            var hasAccess = await _roleService.HasPermissionAsync(userRoleIds, _feature);

            if (!hasAccess)
            {
                context.Result = new ForbidResult();
            }

        }
    }
}

using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Attributes
{
    public class HasPermissionAttribute : TypeFilterAttribute
    {
        public HasPermissionAttribute(Feature feature) : base(typeof(PermissionAuthorizationFilter))
        {
            Arguments = new object[] { feature };
        }
    }
}

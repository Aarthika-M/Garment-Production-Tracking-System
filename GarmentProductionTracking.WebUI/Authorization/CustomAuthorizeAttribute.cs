using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Domain.Interfaces;

namespace WebUI.Authorization
{
    public class CustomAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(role))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                return;
            }

            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

           
            if (role == "Manager" && controller == "AccessControl")
                return;

            // Repository
            var repo = context.HttpContext.RequestServices
                .GetService(typeof(IRolePermissionRepository)) as IRolePermissionRepository;

            if (repo == null)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                return;
            }

           
            var allPermissions = await repo.GetPermissionsByRoleAsync(role);
            bool dbEmpty = allPermissions == null || allPermissions.Count == 0;

           
            if (role == "Manager" && dbEmpty)
                return;

           
            var permission = await repo.GetPermissionAsync(role, controller, action);

            if (permission == null || !permission.Enabled)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }
        }
    }
}

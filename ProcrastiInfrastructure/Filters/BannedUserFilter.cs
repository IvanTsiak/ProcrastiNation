using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProcrastiDomain.Model;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProcrastiInfrastructure.Filters
{
    public class BannedUserFilter : IAsyncActionFilter
    {
        private readonly ProcrastiContext _context;

        public BannedUserFilter(ProcrastiContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                var path = context.HttpContext.Request.Path.Value?.ToLower();

                if (path != null && !path.Contains("/account/banned")  && !path.Contains("/account/logout"))
                {
                    var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                    {
                        var user = await _context.Users.FindAsync(userId);
                        if (user != null && (user.Isbanned ?? false))
                        {
                            context.Result = new RedirectToActionResult("Banned", "Account", null);
                            return;
                        }
                    }
                }
            }

            await next();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Threading.Tasks;
using ProcrastiInfrastructure.Shared;

namespace ProcrastiInfrastructure.Filters
{
    public class BannedUserFilter : IAsyncActionFilter
    {
        private readonly ProcrastiContext _context;
        private readonly IMemoryCache _cache;
        public BannedUserFilter(ProcrastiContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                var path = context.HttpContext.Request.Path.Value?.ToLower();

                if (path != null && !path.Contains(Constants.Paths.Banned) && !path.Contains(Constants.Paths.Logout))
                {
                    var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                    {
                        string cacheKey = $"UserBanStatus_{userId}";

                        bool isBanned = await _cache.GetOrCreateAsync(cacheKey, async entry =>
                        {
                            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Constants.Limits.CheckBanFreqMinutes);

                            return await _context.Users
                                .Where(u => u.Id == userId)
                                .Select(u => u.Isbanned ?? false)
                                .FirstOrDefaultAsync();
                        });

                        if (isBanned)
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
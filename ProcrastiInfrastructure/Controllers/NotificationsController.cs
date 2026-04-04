using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcrastiDomain.Model;
using ProcrastiInfrastructure.Services;

namespace ProcrastiInfrastructure.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly ProcrastiContext _context;
        private readonly ICurrentUserService _currentUserService;

        public NotificationsController(ProcrastiContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int userId = _currentUserService.GetCurrentUserId();

            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            var unreadNotifications = notifications.Where(n => !n.Isviewed).ToList();
            if (unreadNotifications.Any())
            {
                foreach (var notif in unreadNotifications)
                {
                    notif.Isviewed = true;
                }
                
                await _context.SaveChangesAsync();
            }

            return View(notifications);
        }
    }
}

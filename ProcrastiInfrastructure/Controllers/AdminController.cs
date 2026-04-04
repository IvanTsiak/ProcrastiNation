using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProcrastiDomain.Model;
using ProcrastiInfrastructure;
using ProcrastiInfrastructure.Services;
using System.Linq;
using System.Threading.Tasks;

namespace ProcrastiInfrastructure.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ProcrastiContext _context;
        private readonly INotificationService _notificationService;

        public AdminController(ProcrastiContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users
                .Include(u => u.Title)
                .OrderByDescending(u => u.Username)
                .ToListAsync();

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleBlock(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Isbanned = !(user.Isbanned ?? false);
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> AwardTitle(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AwardTitle(int userId, string titleCode, string titleName)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(titleCode) || string.IsNullOrWhiteSpace(titleName)) {
                ModelState.AddModelError("", "Код і назва титулу обов'язкові! Як він буде його носити? Думай головою.");
                return View(user);
            }

            var newTitle = new Title
            {
                Code = titleCode,
                Name = titleName,
                Isunique = true
            };

            _context.Titles.Add(newTitle);
            await _context.SaveChangesAsync();

            var userTitle = new Usertitle
            {
                Userid = userId,
                Titleid = newTitle.Id,
            };

            _context.Usertitles.Add(userTitle);
            await _context.SaveChangesAsync();

            await _notificationService.AddNotificationAsync(
                userId,
                $"НЕПЕРЕВЕРШЕНО! Адміністратор нагородив вас УНІКАЛЬНИМ титулом: {newTitle.Name}!",
                "Унікальний титул розблоковано!",
                "Title",
                "/Titles"
            );

            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> Analytics()
        {
            var todayUtc = DateTime.UtcNow.Date;
            var today = DateTime.SpecifyKind(todayUtc, DateTimeKind.Unspecified);
            var globalStat = await _context.Globalstats.FirstOrDefaultAsync();

            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.NewUsersToday = await _context.Users.CountAsync(u => u.Joineddate >= today);

            ViewBag.TotalLogs = await _context.Logs.CountAsync();
            ViewBag.LogsToday = await _context.Logs.CountAsync(l => l.Createdat >= today);

            ViewBag.TotalWastedMinutes = globalStat?.Totallossamount ?? 0;

            ViewBag.LossCount = await _context.Logs.CountAsync(l => l.Logtype == LogType.loss);
            ViewBag.GainCount = await _context.Logs.CountAsync(l => l.Logtype == LogType.win);

            return View();
        }
    }
}

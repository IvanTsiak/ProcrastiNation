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

        [HttpGet]
        public async Task<IActionResult> SendNotification(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendNotification(int userId, string title, string message)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(title)) {
                ModelState.AddModelError("", "Повідомлення та заголовок обов'язкові! Що ти хочеш їм сказати? Думай головою.");
                return View(user);
            }

            await _notificationService.AddNotificationAsync(
                userId,
                message,
                title,
                "AdminMessage"
            );

            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> Logs()
        {
            var pendingLogs = await _context.Logs
                .Include(l => l.User)
                    .ThenInclude(u => u.Title)
                .Include(l => l.Activity)
                .Where(l => l.Isvisible == false)
                .OrderByDescending(l => l.Createdat)
                .ToListAsync();

            return View(pendingLogs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveLog(int id)
        {
            var log = await _context.Logs.FindAsync(id);
            if (log != null && !log.Isvisible)
            {
                log.Isvisible = true;
                if (log.Logtype == LogType.loss)
                {
                    var globalStat = await _context.Globalstats.FirstOrDefaultAsync();
                    if (globalStat != null)
                    {
                        globalStat.Totallossamount = (globalStat.Totallossamount ?? 0) + log.Amount;
                        globalStat.Lastupdated = DateTime.Now;
                        _context.Update(globalStat);
                    }
                }
                _context.Update(log);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Logs));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLog(int id)
        {
            var log = await _context.Logs.FindAsync(id);
            if (log != null)
            {
                if (log.Activityid.HasValue)
                {
                    var activity = await _context.Activities.FindAsync(log.Activityid);
                    if (activity != null)
                    {
                        activity.Mentionscount = Math.Max(0, (activity.Mentionscount ?? 1) - 1);
                        _context.Update(activity);
                    }
                }

                _context.Logs.Remove(log);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Logs));
        }
    }
}

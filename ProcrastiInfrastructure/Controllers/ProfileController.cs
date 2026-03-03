using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcrastiDomain.Model;
using ProcrastiInfrastructure.Models;
using ProcrastiInfrastructure.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProcrastiInfrastructure.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ProcrastiContext _context;
        private readonly ICurrentUserService _currentUserService;

        public ProfileController(ProcrastiContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<IActionResult> Index()
        {
            int userId = _currentUserService.GetCurrentUserId();

            var user = await _context.Users
                .Include(u => u.Title)
                .Include(u => u.Userachievements)
                .Include(u => u.Usertitles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileViewModel
            {
                Username = user.Username ?? "Anonym",
                ProfilePicture = user.Profilepicture ?? "/images/avatars/default-avatar.png",
                CurrentTitle = user.Title?.Name ?? "No Title",
                JoinedDate = user.Joineddate,
                AchievementsCount = user.Userachievements?.Count ?? 0,
                TitlesCount = user.Usertitles?.Count ?? 0,
                TotalLoss = user.Totalloss ?? 0
            };

            viewModel.RecentLogs = await _context.Logs
                .Include(l => l.Activity)
                .Include(l => l.User)
                .Include(l => l.Likes)
                .Where(l => l.Userid == userId)
                .OrderByDescending(l => l.Createdat)
                .Take(3)
                .ToListAsync();

            var today = DateTime.Now;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);

            var monthLogs = await _context.Logs
                .Where(l => l.Userid == userId &&
                l.Logtype == LogType.loss &&
                l.Createdat >= startOfMonth &&
                l.Createdat < startOfNextMonth)
                .ToListAsync();

            viewModel.MonthlyLossData = monthLogs
                .Where(l => l.Createdat.HasValue)
                .GroupBy(l => l.Createdat.Value.Date)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key.ToString("dd.MM.yyyy"), g => g.Sum(l => l.Amount));

            return View(viewModel);
        }
    }
}

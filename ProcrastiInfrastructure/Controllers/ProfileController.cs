using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcrastiDomain.Model;
using ProcrastiInfrastructure.Models;
using ProcrastiInfrastructure.Services;
using ProcrastiInfrastructure.Shared;

namespace ProcrastiInfrastructure.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ProcrastiContext _context;
        private readonly ICurrentUserService _currentUserService;

        public ProfileController(ProcrastiContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<IActionResult> Index(int? id)
        {
            int currentUserId = _currentUserService.GetCurrentUserId();

            int targetUserId = id ?? currentUserId;
            bool isCurrentUser = targetUserId == currentUserId;

            var user = await _context.Users
                .Include(u => u.Title)
                .Include(u => u.Userachievements)
                .Include(u => u.Usertitles)
                .FirstOrDefaultAsync(u => u.Id == targetUserId);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileViewModel
            {
                Username = user.Username ?? Constants.Unknown.UnkUser,
                ProfilePicture = user.Profilepicture ?? Constants.Paths.DefaultProfilePicture,
                CurrentTitle = user.Title?.Name ?? Constants.Unknown.UnkTitle,
                JoinedDate = user.Joineddate,
                AchievementsCount = user.Userachievements?.Count ?? 0,
                TitlesCount = user.Usertitles?.Count ?? 0,
                TotalLoss = user.Totalloss ?? 0,
                IsCurrentUser = isCurrentUser
            };

            viewModel.RecentLogs = await _context.Logs
                .Include(l => l.Activity)
                .Include(l => l.User)
                    .ThenInclude(u => u.Title)
                .Include(l => l.Likes)
                .Include(l => l.Comments)
                    .ThenInclude(c => c.Author)
                        .ThenInclude(a => a.Title)
                .Where(l => l.Userid == targetUserId)
                .OrderByDescending(l => l.Createdat)
                .Take(3)
                .ToListAsync();

            var today = DateTime.Now;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);

            viewModel.MonthlyLossData = await _context.Logs
                .Where(l => l.Userid == targetUserId &&
                            l.Logtype == LogType.loss &&
                            l.Createdat >= startOfMonth &&
                            l.Createdat < startOfNextMonth &&
                            l.Createdat.HasValue)
                .GroupBy(l => l.Createdat.Value.Date)
                .Select(g => new { Date = g.Key, TotalAmount = g.Sum(l => l.Amount) })
                .ToDictionaryAsync(g => g.Date.ToString("dd.MM.yyyy"), g => g.TotalAmount);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AllLogs()
        {
            int userId = _currentUserService.GetCurrentUserId();
            var logs = await _context.Logs
                .Include(l => l.Activity)
                .Include(l => l.User)
                    .ThenInclude(u => u.Title)
                .Include(l => l.Likes)
                .Include(l => l.Comments)
                    .ThenInclude(c => c.Author)
                        .ThenInclude(a => a.Title)
                .Where(l => l.Userid == userId)
                .OrderByDescending(l => l.Createdat)
                .ToListAsync();
            return View(logs);
        }
    }
}

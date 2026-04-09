using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcrastiInfrastructure.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProcrastiInfrastructure.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ProcrastiContext _context;

        public HomeController(ProcrastiContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder = "newest")
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                var latestLogs = await _context.Logs
                .Include(l => l.User).ThenInclude(u => u.Title)
                .Include(l => l.Activity)
                .Include(l => l.Likes)
                .Include(l => l.Comments).ThenInclude(c => c.Author).ThenInclude(a => a.Title)
                .Where(l => l.Isvisible == true)
                .OrderByDescending(l => l.Createdat)
                .Take(4)
                .ToListAsync();

                return View("Landing", latestLogs);
            }

            var viewModel = new DashboardViewModel
            {
                CurrentSort = sortOrder
            };

            var globalStat = await _context.Globalstats.FirstOrDefaultAsync();
            viewModel.GlobalLossAmount = globalStat != null ? globalStat.Totallossamount ?? 0 : 0;

            var logsQuery = _context.Logs
                .Include(l => l.User)
                    .ThenInclude(u => u.Title)
                .Include(l => l.Activity)
                .Include(l => l.Likes)
                .Include(l => l.Comments)
                    .ThenInclude(c => c.Author)
                        .ThenInclude(a => a.Title)
                .Where(log => log.Isvisible == true);
            
            switch (sortOrder)
            {
                case "popular":
                    logsQuery = logsQuery.OrderByDescending(l => l.Likescount ?? 0).ThenByDescending(l => l.Createdat);
                    break;
                case "oldest":
                    logsQuery = logsQuery.OrderBy(l => l.Createdat);
                    break;
                case "newest":
                default:
                    logsQuery = logsQuery.OrderByDescending(l => l.Createdat);
                    break;
            }

            viewModel.RecentLogs = await logsQuery.Take(100).ToListAsync();

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

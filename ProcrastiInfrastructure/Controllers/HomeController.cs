using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcrastiInfrastructure.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProcrastiInfrastructure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProcrastiContext _context;

        public HomeController(ProcrastiContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel();

            var globalStat = await _context.Globalstats.FirstOrDefaultAsync();
            viewModel.GlobalLossAmount = globalStat != null ? globalStat.Totallossamount ?? 0 : 0;

            viewModel.RecentLogs = await _context.Logs
                .Include(l => l.User)
                .Include(l => l.Activity)
                .OrderByDescending(log => log.Createdat)
                .Take(100)
                .ToListAsync();

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

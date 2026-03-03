using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ProcrastiDomain.Model;
using ProcrastiInfrastructure.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProcrastiInfrastructure.Controllers
{
    public class GlobalController : Controller
    {
        private readonly ProcrastiContext _context;

        public GlobalController(ProcrastiContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new GlobalStatsViewModel();

            viewModel.TopComments = await _context.Logs
                .Include(l => l.User)
                .Include(l => l.Activity)
                .Where(l => !string.IsNullOrEmpty(l.Comment) && l.Isvisible == true)
                .OrderByDescending(l => l.Likescount)
                .Take(5)
                .ToListAsync();

            viewModel.TopUsers = await _context.Users
                .OrderByDescending(u => u.Totalloss)
                .Take(3)
                .ToListAsync();

            viewModel.TopActivities = await _context.Activities
                .OrderByDescending(a => a.Mentionscount)
                .Take(3)
                .ToListAsync();

            return View(viewModel);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ProcrastiDomain.Model;
using ProcrastiInfrastructure.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProcrastiInfrastructure.Controllers
{
    [Authorize]
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
                    .ThenInclude(u => u.Title)
                .Include(l => l.Activity)
                .Include(l => l.Likes)
                .Include(l => l.Comments)
                    .ThenInclude(c => c.Author)
                        .ThenInclude(a => a.Title)
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
                .Take(6)
                .ToListAsync();

            return View(viewModel);
        }
    }
}

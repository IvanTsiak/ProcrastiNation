using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcrastiInfrastructure.Models;
using ProcrastiInfrastructure.Shared;

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
                .Take(Constants.Limits.TopComents)
                .ToListAsync();

            viewModel.TopUsers = await _context.Users
                .OrderByDescending(u => u.Totalloss)
                .Take(Constants.Limits.TopUsers)
                .ToListAsync();

            viewModel.TopActivities = await _context.Activities
                .OrderByDescending(a => a.Mentionscount)
                .Take(Constants.Limits.TopActivities)
                .ToListAsync();

            return View(viewModel);
        }
    }
}

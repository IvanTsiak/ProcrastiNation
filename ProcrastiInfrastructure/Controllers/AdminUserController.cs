using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProcrastiDomain.Model;
using ProcrastiInfrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace ProcrastiInfrastructure.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly ProcrastiContext _context;

        public AdminUserController(ProcrastiContext context)
        {
            _context = context;
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

            return RedirectToAction(nameof(Users));
        }
    }
}

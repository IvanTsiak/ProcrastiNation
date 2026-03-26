using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProcrastiDomain.Model;
using ProcrastiInfrastructure;
using ProcrastiInfrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcrastiInfrastructure.Controllers
{
    [Authorize]
    public class TitlesController : Controller
    {
        private readonly ProcrastiContext _context;
        private readonly ICurrentUserService _currentUserService;

        public TitlesController(ProcrastiContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        // GET: Titles
        public async Task<IActionResult> Index()
        {
            int currentUserId = _currentUserService.GetCurrentUserId();

            var userTitles = await _context.Usertitles
                .Include(ut => ut.Title)
                .Where(ut => ut.Userid == currentUserId)
                .Select(ut => ut.Title)
                .ToListAsync();

            var user = await _context.Users.FindAsync(currentUserId);
            ViewBag.ActiveTitleId = user?.Titleid;

            return View(userTitles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActive(int? titleId)
        {
            int currentUserId = _currentUserService.GetCurrentUserId();
            var user = await _context.Users.FindAsync(currentUserId);

            if (user != null)
            {
                if (titleId.HasValue)
                {
                    bool ownsTitle = await _context.Usertitles
                        .AnyAsync(ut => ut.Userid == currentUserId && ut.Titleid == titleId.Value);

                    if (ownsTitle)
                    {
                        user.Titleid = titleId.Value;
                    }
                }
                else
                {
                    user.Titleid = null;
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Profile");
        }


        // GET: Titles/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var title = await _context.Titles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (title == null)
            {
                return NotFound();
            }

            return View(title);
        }

        // GET: Titles/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Titles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,Name,Isunique,Id")] Title title)
        {
            if (ModelState.IsValid)
            {
                _context.Add(title);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(title);
        }

        // GET: Titles/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var title = await _context.Titles.FindAsync(id);
            if (title == null)
            {
                return NotFound();
            }
            return View(title);
        }

        // POST: Titles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Code,Name,Isunique,Id")] Title title)
        {
            if (id != title.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(title);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TitleExists(title.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(title);
        }

        // GET: Titles/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var title = await _context.Titles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (title == null)
            {
                return NotFound();
            }

            return View(title);
        }

        // POST: Titles/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var title = await _context.Titles.FindAsync(id);
            if (title != null)
            {
                _context.Titles.Remove(title);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TitleExists(int id)
        {
            return _context.Titles.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Unlock([FromBody] string code)
        {
            int currentUserId = _currentUserService.GetCurrentUserId();

            var title = await _context.Titles.FirstOrDefaultAsync(t => t.Code == code);
            if (title == null)
            {
                return NotFound("Title not found.");
            }

            bool alreadyUnlocked = await _context.Usertitles
                .AnyAsync(ut => ut.Userid == currentUserId && ut.Titleid == title.Id);

            if (alreadyUnlocked)
            {
                return BadRequest("Title already unlocked.");
            }

            var newTitleUnlock = new Usertitle
            {
                Userid = currentUserId,
                Titleid = title.Id,
                Unlockedat = DateTime.Now
            };

            _context.Usertitles.Add(newTitleUnlock);
            await _context.SaveChangesAsync();

            return Json(new
            {
                name = title.Name
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Manage() 
        {
            var titles = await _context.Titles.ToListAsync();

            return View(titles);
        }
    }
}

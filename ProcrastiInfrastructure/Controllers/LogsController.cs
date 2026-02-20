using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProcrastiDomain.Model;
using ProcrastiInfrastructure;

namespace ProcrastiInfrastructure.Controllers
{
    public class LogsController : Controller
    {
        private readonly ProcrastiContext _context;

        public LogsController(ProcrastiContext context)
        {
            _context = context;
        }

        // GET: Logs
        public async Task<IActionResult> Index()
        {
            var procrastiContext = _context.Logs.Include(l => l.Activity).Include(l => l.User);
            return View(await procrastiContext.ToListAsync());
        }

        // GET: Logs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Logs
                .Include(l => l.Activity)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        // GET: Logs/Create
        public IActionResult Create()
        {
            ViewData["Activityid"] = new SelectList(_context.Activities, "Id", "Name");
            ViewData["Userid"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["Logtype"] = new SelectList(Enum.GetValues(typeof(LogType)));
            return View();
        }

        // POST: Logs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Userid,Activityid,Logtype,Amount,Rating,Comment,Createdat,Isvisible,Likescount,Id")] Log log)
        {
            log.Userid = 1;
            log.Createdat = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(log);

                var user = await _context.Users.FindAsync(log.Userid);
                if (user != null && log.Logtype == LogType.loss)
                {
                    user.Totalloss += log.Amount;
                    _context.Update(user);
                }

                var activity = await _context.Activities.FindAsync(log.Activityid);
                if (activity != null)
                {
                    activity.Mentionscount += 1;
                    _context.Update(activity);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Activityid"] = new SelectList(_context.Activities, "ActivityId", "Name", log.Activityid);
            return View(log);
        }

        // GET: Logs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Logs.FindAsync(id);
            if (log == null)
            {
                return NotFound();
            }
            ViewData["Activityid"] = new SelectList(_context.Activities, "Id", "Name", log.Activityid);
            ViewData["Userid"] = new SelectList(_context.Users, "Id", "Email", log.Userid);
            return View(log);
        }

        // POST: Logs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Userid,Activityid,Logtype,Amount,Rating,Comment,Createdat,Isvisible,Likescount,Id")] Log log)
        {
            if (id != log.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(log);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogExists(log.Id))
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
            ViewData["Activityid"] = new SelectList(_context.Activities, "Id", "Name", log.Activityid);
            ViewData["Userid"] = new SelectList(_context.Users, "Id", "Email", log.Userid);
            return View(log);
        }

        // GET: Logs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Logs
                .Include(l => l.Activity)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        // POST: Logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var log = await _context.Logs.FindAsync(id);
            if (log != null)
            {
                _context.Logs.Remove(log);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogExists(int id)
        {
            return _context.Logs.Any(e => e.Id == id);
        }
    }
}

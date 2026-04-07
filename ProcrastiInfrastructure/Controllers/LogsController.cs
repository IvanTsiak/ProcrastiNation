using DocumentFormat.OpenXml.Office2013.PowerPoint;
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
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace ProcrastiInfrastructure.Controllers
{
    [Authorize]
    public class LogsController : Controller
    {
        private readonly ProcrastiContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationService _notificationService;

        public LogsController(ProcrastiContext context, ICurrentUserService currentUserService, INotificationService notificationService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _notificationService = notificationService;
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
            ViewBag.VerifiedActivityNames = _context.Activities
                 .Where(a => a.Isverified == true)
                 .Select(a => a.Name)
                 .ToList();

            var logTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = LogType.win.ToString(), Text = "Win" },
                new SelectListItem { Value = LogType.loss.ToString(), Text = "Loss" }
            };
            ViewData["Logtype"] = logTypes;

            return View();
        }

        // POST: Logs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Logtype,Amount,Rating,Comment")] Log log, string activityName)
        {
            log.Userid = _currentUserService.GetCurrentUserId();
            log.Createdat = DateTime.Now;
            log.Isvisible = true;
            log.Likescount = 0;

            if (string.IsNullOrWhiteSpace(activityName))
            {
                ModelState.AddModelError("activityName", "You must specify an activity.");
            }

            if (ModelState.IsValid)
            {
                var existingActivity = await _context.Activities
                    .FirstOrDefaultAsync(a => a.Name.ToLower() == activityName.ToLower());

                if (existingActivity != null)
                {
                    log.Activityid = existingActivity.Id;
                    existingActivity.Mentionscount = (existingActivity.Mentionscount ?? 0) + 1;
                    _context.Update(existingActivity);
                }
                else
                {
                    var newActivity = new Activity
                    {
                        Name = activityName,
                        Isverified = false,
                        Mentionscount = 1
                    };
                    _context.Activities.Add(newActivity);
                    await _context.SaveChangesAsync();

                    log.Activityid = newActivity.Id;
                }
                
                _context.Add(log);

                var user = await _context.Users.FindAsync(log.Userid);
                if (user != null && log.Logtype == LogType.loss)
                {
                    user.Totalloss += log.Amount;
                    _context.Update(user);

                    if (user.Totalloss >= 1440)
                    {
                        TempData["PendingTitle"] = "PROcrastinator";
                    }
                }

                if (log.Logtype == LogType.loss)
                {
                    var globalStat = await _context.Globalstats.FirstOrDefaultAsync();

                    if (globalStat != null)
                    {
                        globalStat.Totallossamount += log.Amount;
                        globalStat.Lastupdated = DateTime.Now;
                        _context.Update(globalStat);
                    }
                    else
                    {
                        var newGlobalStat = new Globalstat
                        {
                            Totallossamount = log.Amount,
                            Lastupdated = DateTime.Now
                        };
                        _context.Globalstats.Add(newGlobalStat);
                    }
                }

                if (log.Logtype == LogType.loss && log.Amount >= 300)
                {
                    TempData["PendingAchievement"] = "SURVIVOR";
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            ViewBag.VerifiedActivityNames = _context.Activities
                 .Where(a => a.Isverified == true)
                 .Select(a => a.Name)
                 .ToList();

            ViewData["Logtype"] = new List<SelectListItem>
            {
                new SelectListItem { Value = LogType.win.ToString(), Text = "Win" },
                new SelectListItem { Value = LogType.loss.ToString(), Text = "Loss" }
            };
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

            var logTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = LogType.win.ToString(), Text = "Win" },
                new SelectListItem { Value = LogType.loss.ToString(), Text = "Loss" }
            };
            ViewData["Logtype"] = logTypes;
            return View(log);
        }

        // POST: Logs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Activityid,Logtype,Amount,Rating,Comment,Createdat")] Log log)
        {
            if (id != log.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dbLog = await _context.Logs.FirstOrDefaultAsync(l => l.Id == id);

                    if (dbLog != null)
                    {
                        if (dbLog.Userid != _currentUserService.GetCurrentUserId())
                        {
                            return Unauthorized();
                        }

                        int lossDifference = 0;

                        if (dbLog.Logtype == LogType.loss && log.Logtype == LogType.loss)
                        {
                            lossDifference = log.Amount - dbLog.Amount;
                        }
                        else if (dbLog.Logtype == LogType.win && log.Logtype == LogType.loss)
                        {
                            lossDifference = log.Amount;

                        }
                        else if (dbLog.Logtype == LogType.loss && log.Logtype == LogType.win)
                        {
                            lossDifference = -dbLog.Amount;
                        }

                        if (lossDifference != 0)
                        {
                            var user = await _context.Users.FindAsync(log.Userid);
                            if (user != null)
                            {
                                user.Totalloss = Math.Max(0, (user.Totalloss ?? 0) + lossDifference);
                                _context.Update(user);
                            }
                            var globalStat = await _context.Globalstats.FirstOrDefaultAsync();
                            if (globalStat != null)
                            {
                                globalStat.Totallossamount = Math.Max(0, (globalStat.Totallossamount ?? 0) + lossDifference);
                                globalStat.Lastupdated = DateTime.Now;
                                _context.Update(globalStat);
                            }
                        }

                        if (dbLog.Activityid != log.Activityid)
                        {
                            if (dbLog.Activityid.HasValue)
                            {
                                var oldActivity = await _context.Activities.FindAsync(dbLog.Activityid);
                                if (oldActivity != null)
                                {
                                    oldActivity.Mentionscount = Math.Max(0, (oldActivity.Mentionscount ?? 1) - 1);
                                    _context.Update(oldActivity);
                                }
                            }
                            if (log.Activityid.HasValue)
                            {
                                var newActivity = await _context.Activities.FindAsync(log.Activityid);
                                if (newActivity != null)
                                {
                                    newActivity.Mentionscount = (newActivity.Mentionscount ?? 0) + 1;
                                    _context.Update(newActivity);
                                }
                            }
                        }
                    }

                    dbLog.Activityid = log.Activityid;
                    dbLog.Logtype = log.Logtype;
                    dbLog.Amount = log.Amount;
                    dbLog.Rating = log.Rating;
                    dbLog.Comment = log.Comment;
                    dbLog.Createdat = log.Createdat;

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
            ViewData["Logtype"] = new List<SelectListItem>
            {
                new SelectListItem { Value = LogType.win.ToString(), Text = "Win" },
                new SelectListItem { Value = LogType.loss.ToString(), Text = "Loss" }
            };
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
                if (log.Activityid.HasValue)
                {
                    var activity = await _context.Activities.FindAsync(log.Activityid);
                    if (activity != null)
                    {
                        activity.Mentionscount = Math.Max(0, (activity.Mentionscount ?? 1) - 1);
                        _context.Update(activity);
                    }
                }

                _context.Logs.Remove(log);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LogExists(int id)
        {
            return _context.Logs.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLike(int id)
        {
            int currentUserId = _currentUserService.GetCurrentUserId();

            var log = await _context.Logs
                .Include(l => l.Activity)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (log == null)
            {
                return NotFound(); 
            }

            var existingLike = await _context.Likes.FirstOrDefaultAsync(l => l.Logid == id && l.Userid == currentUserId);

            bool isLikedNow;

            if (existingLike != null)
            {
                _context.Likes.Remove(existingLike);

                log.Likescount = Math.Max(0, (log.Likescount ?? 1) - 1);
                isLikedNow = false;
            }
            else
            {
                var newLike = new Like
                {
                    Userid = currentUserId,
                    Logid = id
                };
                _context.Likes.Add(newLike);

                log.Likescount = (log.Likescount ?? 0) + 1;
                isLikedNow = true;
            }

            await _context.SaveChangesAsync();

            if (isLikedNow && log.Userid.HasValue && log.Userid != currentUserId)
            {
                var liker = await _context.Users.FindAsync(currentUserId);
                string likerName = liker?.Username ?? "Хрін зна хто";
                string activityName = log.Activity?.Name ?? "запис";
                int amount = log.Amount;

                await _notificationService.AddNotificationAsync(
                    log.Userid.Value,
                    $"{likerName} вподобав ваш запис у \"{activityName}\" ({amount} хв).",
                    "Новий лайк!",
                    "Like",
                    $"/Profile/AllLogs#log-{id}"
                );
            }

            return Json(new { newLikesCount = log.Likescount, isLiked = isLikedNow });
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromForm] int logId, [FromForm] string text, [FromForm] int? parentCommentId = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest("Comment text cannot be empty.");
            }

            int currentUserId = _currentUserService.GetCurrentUserId();

            var newComment = new Comment
            {
                Logid = logId,
                Authorid = currentUserId,
                Content = text,
                Parentcommentid = parentCommentId,
                Createdat = DateTime.Now
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            var user = await _context.Users
                .Include(u => u.Title)
                .FirstOrDefaultAsync(u => u.Id == currentUserId);

            string commenterName = user?.Username ?? "Хрін зна хто";

            if (parentCommentId.HasValue)
            {
                var parentComment = await _context.Comments.FindAsync(parentCommentId.Value);
                if (parentComment != null && parentComment.Authorid.HasValue && parentComment.Authorid.Value != currentUserId)
                {
                    var log = await _context.Logs
                        .Include(l => l.Activity)
                        .FirstOrDefaultAsync(l => l.Id == logId);
                    string activityInfo = log != null ? $" (під записом \"{log.Activity?.Name}\" {log.Amount} хв)" : "";

                    string parentText = parentComment.Content.Length > 20 ? parentComment.Content.Substring(0, 20) + "..." : parentComment.Content;
                    string replyText = text.Length > 30 ? text.Substring(0, 30) + "..." : text;

                    // Не буде працювати лінк на запис, під яким залишили коментар, якщо коментар залишили під записом не користувача, який отримає сповіщення
                    // Мені лінь щось з цим робити, та і я не знаю, що саме робити
                    await _notificationService.AddNotificationAsync(
                        parentComment.Authorid.Value,
                        $"{commenterName} відповів на ваш коментар \"{parentText}\"{activityInfo}: \"{replyText}\"",
                        "Нова відповідь!",
                        "Reply",
                        $"/Profile/AllLogs#log-{logId}"
                    );
                }
            }
            else
            {
                var log = await _context.Logs
                    .Include(l => l.Activity)
                    .FirstOrDefaultAsync(l => l.Id == logId);
                if (log != null && log.Userid.HasValue && log.Userid.Value != currentUserId)
                {
                    string activityName = log.Activity?.Name ?? "запис";
                    string commentText = text.Length > 30 ? text.Substring(0, 30) + "..." : text;

                    await _notificationService.AddNotificationAsync(
                        log.Userid.Value,
                        $"{commenterName} прокоментував ваш запис \"{activityName}\" ({log.Amount} хв): \"{commentText}\"",
                        "Новий коментар!",
                        "Comment",
                        $"/Profile/AllLogs#log-{logId}"
                    );
                }
            }

            return Json(new
            {
                id = newComment.Id,
                username = user?.Username ?? "Анонім",
                parentCommentId = newComment.Parentcommentid,
                title = user?.Title != null ? user.Title.Name : "",
                text = newComment.Content,
                date = newComment.Createdat?.ToString("dd.MM.yyyy"),
                profilePicture = string.IsNullOrEmpty(user?.Profilepicture) ? "/images/avatars/default-avatar.png" : user.Profilepicture
            });
        }

        [HttpGet]
        public IActionResult CancelCreation()
        {
            TempData["PendingTitle"] = "COWARD";

            return RedirectToAction("Index", "Home");
        }
    }
}

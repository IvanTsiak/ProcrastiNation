using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcrastiDomain.Model;
using ProcrastiInfrastructure.Services;
using System.Security.Claims;

namespace ProcrastiInfrastructure.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ProcrastiContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDataPortServiceFactory<Log> _dataPortServiceFactory;

        public SettingsController(ProcrastiContext context ,ICurrentUserService currentUserService ,IDataPortServiceFactory<Log> dataPortServiceFactory)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dataPortServiceFactory = dataPortServiceFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Export([FromQuery] string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", CancellationToken cancellationToken = default)
        {
            var exportService = _dataPortServiceFactory.GetExportService(contentType);
            var memoryStream = new MemoryStream();

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await exportService.WriteToAsync(memoryStream, userId, cancellationToken);
            await memoryStream.FlushAsync(cancellationToken);
            memoryStream.Position = 0;

            return new FileStreamResult(memoryStream, contentType)
            {
                FileDownloadName = $"procrasti_logs_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx"
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel, CancellationToken cancellationToken = default)
        {
            if (fileExcel == null || fileExcel.Length == 0)
            {
                TempData["ErrorMessage"] = "Будь ласка, оберіть файл.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var expectedContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var importService = _dataPortServiceFactory.GetImportService(expectedContentType);

                using var stream = fileExcel.OpenReadStream();

                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await importService.ImportFromStreamAsync(stream, userId, cancellationToken);

                TempData["SuccessMessage"] = "Дані успішно імпортовано!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Помилка імпорту: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            int currentUserId = _currentUserService.GetCurrentUserId();
            var user = await _context.Users.FindAsync(currentUserId);

            if (user == null)
            {
                return NotFound();
            }
            
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(string username, IFormFile? avatarFile)
        {
            int currentUserId = _currentUserService.GetCurrentUserId();
            var user = await _context.Users.FindAsync(currentUserId);

            if (user == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                ModelState.AddModelError("Username", "Username cannot be empty.");
                return View(user);
            }

            user.Username = username.Trim();

            if (avatarFile != null && avatarFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(avatarFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("AvatarFile", "Unsupported file type. Allowed types: .jpg, .jpeg, .png, .gif");
                    return View(user);
                }

                string fileName = $"avatar_{currentUserId}_{Guid.NewGuid()}{extension}";

                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "avatars");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
                }

                user.Profilepicture = $"/images/avatars/{fileName}";
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Profile");
        }
    }
}

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
        private readonly IDataPortServiceFactory<Log> _dataPortServiceFactory;

        public SettingsController(IDataPortServiceFactory<Log> dataPortServiceFactory)
        {
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
    }
}

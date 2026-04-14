using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using ProcrastiDomain.Model;
using ProcrastiInfrastructure.Shared;

namespace ProcrastiInfrastructure.Services
{
    public class LogExportService : IExportService<Log>
    {
        private readonly ProcrastiContext _context;
        private static readonly IReadOnlyList<string> HeaderNames = new string[]
        {
            "Дата", "Тип", "Активність", "Категорія", "Витрачено хвилин", "Оцінка", "Коментар"
        };

        public LogExportService(ProcrastiContext context)
        {
            _context = context;
        }

        public async Task WriteToAsync(Stream stream, int userId, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Stream must be writable.", nameof(stream));
            }

            var logs = await _context.Logs
                .Include(l => l.Activity)
                    .ThenInclude(a => a.Category)
                .Where(l => l.Userid == userId)
                .OrderByDescending(l => l.Createdat)
                .ToListAsync(cancellationToken);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Мої записи");

            for (int i = 0; i < HeaderNames.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = HeaderNames[i];
            }
            worksheet.Row(1).Style.Font.Bold = true;

            int rowIndex = 2;
            foreach (var log in logs)
            {
                worksheet.Cell(rowIndex, 1).Value = log.Createdat?.ToString("dd.MM.yyyy HH:mm") ?? "";
                worksheet.Cell(rowIndex, 2).Value = log.Logtype == LogType.win ? Constants.LogTypes.WinText : Constants.LogTypes.LossText;
                worksheet.Cell(rowIndex, 3).Value = log.Activity?.Name ?? Constants.Unknown.UnkActivity;
                worksheet.Cell(rowIndex, 4).Value = log.Activity?.Category?.Name ?? Constants.Unknown.UnkCategory;
                worksheet.Cell(rowIndex, 5).Value = log.Amount;
                worksheet.Cell(rowIndex, 6).Value = log.Rating;
                worksheet.Cell(rowIndex, 7).Value = log.Comment ?? "";
                rowIndex++;
            }

            worksheet.Columns().AdjustToContents();
            workbook.SaveAs(stream);
        }
    }
}

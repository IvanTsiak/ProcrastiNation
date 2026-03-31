using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using ProcrastiDomain.Model;
using System.Globalization;

namespace ProcrastiInfrastructure.Services
{
    public class LogImportService : IImportService<Log>
    {
        private readonly ProcrastiContext _context;

        public LogImportService(ProcrastiContext context)
        {
            _context = context;
        }

        public async Task ImportFromStreamAsync(Stream stream, int userId, CancellationToken cancellationToken)
        {
            if (!stream.CanRead) throw new ArgumentException("Дані не можуть бути прочитані", nameof(stream));

            using var workBook = new XLWorkbook(stream);
            var worksheet = workBook.Worksheets.FirstOrDefault();
            if (worksheet == null) return;

            var user = await _context.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user == null) return;

            var globalStat = await _context.Globalstats.FirstOrDefaultAsync(cancellationToken);
            if (globalStat == null)
            {
                globalStat = new Globalstat { Totallossamount = 0 };
                _context.Globalstats.Add(globalStat);
            }

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                await ProcessRowAsync(row, user, globalStat, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task ProcessRowAsync(IXLRow row, User user, Globalstat globalStat, CancellationToken cancellationToken)
        {
            var dateStr = row.Cell(1).Value.ToString().Trim();
            var typeStr = row.Cell(2).Value.ToString().Trim().ToLower();
            var activityName = row.Cell(3).Value.ToString().Trim();
            var categoryName = row.Cell(4).Value.ToString().Trim();

            if (string.IsNullOrEmpty(activityName)) return;

            int amount = row.Cell(5).TryGetValue(out int a) ? a : 0;
            int rating = row.Cell(6).TryGetValue(out int r) ? r : 0;
            string comment = row.Cell(7).Value.ToString();

            DateTime logDate;
            if (!DateTime.TryParseExact(dateStr, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out logDate))
            {
                logDate = DateTime.UtcNow;
            }

            Category category = null;
            if (!string.IsNullOrEmpty(categoryName))
            {
                category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName, cancellationToken);
                if (category == null)
                {
                    category = new Category { Name = categoryName };
                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            var activity = await _context.Activities.FirstOrDefaultAsync(a => a.Name == activityName, cancellationToken);
            if (activity == null)
            {
                activity = new Activity
                {
                    Name = activityName,
                    Categoryid = category?.Id,
                    Isverified = false,
                    Mentionscount = 1
                };
                _context.Activities.Add(activity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                activity.Mentionscount = (activity.Mentionscount ?? 0) + 1;
                _context.Activities.Update(activity);
            }

            var logType = (typeStr == "win" || typeStr == "w") ? LogType.win : LogType.loss;

            if (logType == LogType.loss)
            {
                user.Totalloss = (user.Totalloss ?? 0) + amount;

                globalStat.Totallossamount = (globalStat.Totallossamount ?? 0) + amount;

                globalStat.Lastupdated = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            }

            var log = new Log
            {
                Userid = user.Id,
                Activityid = activity.Id,
                Logtype = logType,
                Amount = amount,
                Rating = rating,
                Comment = comment,
                Isvisible = true,
                Createdat = DateTime.SpecifyKind(logDate, DateTimeKind.Unspecified)
            };

            _context.Logs.Add(log);
        }
    }
}
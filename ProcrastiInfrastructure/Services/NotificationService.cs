using Microsoft.EntityFrameworkCore;
using ProcrastiDomain.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProcrastiInfrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ProcrastiContext _context;

        public NotificationService(ProcrastiContext context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(int userId, string message, string? title = null, string? type = null, string? link = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Title = title,
                Type = type,
                Link = link,
                Isviewed = false,
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            bool needsCleanupSave = false;

            var thirtyDaysAgo = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-30), DateTimeKind.Unspecified);

            var tooOldNotifications = await _context.Notifications
                .Where(n => n.UserId == userId && n.CreatedAt < thirtyDaysAgo)
                .ToListAsync();

            if (tooOldNotifications.Any())
            {
                _context.Notifications.RemoveRange(tooOldNotifications);
                needsCleanupSave = true;
            }

            var excessNotifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Skip(30)
                .ToListAsync();

            if (excessNotifications.Any())
            {
                _context.Notifications.RemoveRange(excessNotifications);
                needsCleanupSave = true;
            }

            if (needsCleanupSave)
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}

using ProcrastiDomain.Model;
using System;
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
        }
    }
}

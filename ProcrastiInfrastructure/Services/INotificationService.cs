using System.Threading.Tasks;

namespace ProcrastiInfrastructure.Services
{
    public interface INotificationService
    {
        Task AddNotificationAsync(int userId, string message, string? title = null, string? type = null, string? link = null);
    }
}

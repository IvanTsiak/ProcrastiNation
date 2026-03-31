using ProcrastiDomain.Model;

namespace ProcrastiInfrastructure.Services
{
    public interface IExportService<TEntity> where TEntity : Entity
    {
        Task WriteToAsync(Stream stream, int userId, CancellationToken cancellationToken);
    }
}

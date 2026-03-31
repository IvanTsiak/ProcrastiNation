using ProcrastiDomain.Model;

namespace ProcrastiInfrastructure.Services
{
    public interface IImportService<TEntity> where TEntity : Entity
    {
        Task ImportFromStreamAsync(Stream stream, int userId, CancellationToken cancellationToken);
    }
}

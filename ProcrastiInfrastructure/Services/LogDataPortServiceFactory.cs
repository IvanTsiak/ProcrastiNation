using ProcrastiDomain.Model;

namespace ProcrastiInfrastructure.Services
{
    public class LogDataPortServiceFactory : IDataPortServiceFactory<Log>
    {
        private readonly ProcrastiContext _context;
        public LogDataPortServiceFactory(ProcrastiContext context)
        {
            _context = context;
        }

        public IImportService<Log> GetImportService(string contentType)
        {
            if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                return new LogImportService(_context);
            throw new NotImplementedException($"No import service implemented for content type {contentType}");
        }

        public IExportService<Log> GetExportService(string contentType)
        {
            if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                return new LogExportService(_context);
            throw new NotImplementedException($"No export service implemented for content type {contentType}");
        }
    }
}

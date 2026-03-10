namespace ProcrastiInfrastructure.Services
{
    public interface ICurrentUserService
    {
        int GetCurrentUserId();
    }

    public class MockCurrentUserService : ICurrentUserService
    {
        public int GetCurrentUserId()
        {
            return 2;
        }
    }
}

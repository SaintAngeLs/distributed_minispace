using MiniSpace.Services.Reports.Application;

namespace MiniSpace.Services.Reports.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}

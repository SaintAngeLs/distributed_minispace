using Pacco.Services.Deliveries.Application;

namespace Pacco.Services.Deliveries.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}
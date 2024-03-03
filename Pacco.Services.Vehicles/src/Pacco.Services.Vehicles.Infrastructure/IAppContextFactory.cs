using Pacco.Services.Vehicles.Application;

namespace Pacco.Services.Vehicles.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}
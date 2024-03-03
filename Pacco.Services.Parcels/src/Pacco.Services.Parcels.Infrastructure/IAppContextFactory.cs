using Pacco.Services.Parcels.Application;

namespace Pacco.Services.Parcels.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}
using Pacco.Services.Orders.Application;

namespace Pacco.Services.Orders.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}
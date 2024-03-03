using Pacco.Services.Customers.Application;

namespace Pacco.Services.Customers.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}
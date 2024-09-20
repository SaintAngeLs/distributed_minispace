using MiniSpace.Services.Communication.Application;

namespace MiniSpace.Services.Communication.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}

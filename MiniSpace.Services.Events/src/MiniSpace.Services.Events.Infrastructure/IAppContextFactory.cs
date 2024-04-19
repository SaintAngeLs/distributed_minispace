using MiniSpace.Services.Events.Application;

namespace MiniSpace.Services.Events.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}
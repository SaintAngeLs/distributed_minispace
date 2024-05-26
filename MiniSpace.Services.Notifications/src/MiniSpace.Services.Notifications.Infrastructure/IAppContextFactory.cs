using MiniSpace.Services.Notifications.Application;

namespace MiniSpace.Services.Notifications.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}

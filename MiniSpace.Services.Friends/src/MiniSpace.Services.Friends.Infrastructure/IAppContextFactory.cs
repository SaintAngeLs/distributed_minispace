using MiniSpace.Services.Friends.Application;

namespace MiniSpace.Services.Friends.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}

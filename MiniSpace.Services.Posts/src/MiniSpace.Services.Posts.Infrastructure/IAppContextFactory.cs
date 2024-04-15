using MiniSpace.Services.Posts.Application;

namespace MiniSpace.Services.Posts.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}

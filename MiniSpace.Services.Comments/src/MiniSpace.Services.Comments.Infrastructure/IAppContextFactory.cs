using MiniSpace.Services.Comments.Application;

namespace MiniSpace.Services.Comments.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}

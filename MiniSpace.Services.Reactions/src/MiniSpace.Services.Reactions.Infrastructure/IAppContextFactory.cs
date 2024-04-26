using MiniSpace.Services.Reactions.Application;

namespace MiniSpace.Services.Reactions.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}

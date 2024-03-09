using MiniSpace.Services.Identity.Application;

namespace MiniSpace.Services.Identity.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}
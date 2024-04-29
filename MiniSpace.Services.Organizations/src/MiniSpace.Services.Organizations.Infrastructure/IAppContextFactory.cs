using MiniSpace.Services.Organizations.Application;

namespace MiniSpace.Services.Organizations.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}

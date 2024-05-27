using MiniSpace.Services.Email.Application;

namespace MiniSpace.Services.Email.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}

using MiniSpace.Services.Students.Application;

namespace MiniSpace.Services.Students.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}

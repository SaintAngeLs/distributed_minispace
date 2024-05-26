using MiniSpace.Services.MediaFiles.Application;

namespace MiniSpace.Services.MediaFiles.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}

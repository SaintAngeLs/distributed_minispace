namespace MiniSpace.Services.MediaFiles.Application
{
    public interface IAppContext
    {
        string RequestId { get; }
        IIdentityContext Identity { get; }
    }    
}

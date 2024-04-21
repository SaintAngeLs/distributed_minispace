namespace MiniSpace.Services.Friends.Application
{
    public interface IAppContext
    {
        string RequestId { get; }
        IIdentityContext Identity { get; }
    }    
}

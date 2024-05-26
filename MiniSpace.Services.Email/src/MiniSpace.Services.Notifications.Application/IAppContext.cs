namespace MiniSpace.Services.Notifications.Application
{
    public interface IAppContext
    {
        string RequestId { get; }
        IIdentityContext Identity { get; }
    }    
}

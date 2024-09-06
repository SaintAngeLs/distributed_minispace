namespace MiniSpace.Services.Communication.Application
{
    public interface IAppContext
    {
        string RequestId { get; }
        IIdentityContext Identity { get; }
    }    
}

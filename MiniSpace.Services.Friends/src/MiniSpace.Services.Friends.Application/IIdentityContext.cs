namespace MiniSpace.Services.Friends.Application
{
    public interface IIdentityContext
    {
        Guid Id { get; }
        string Role { get; }
        string Name { get; }
        string Email { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
        bool IsBanned { get; }
        IDictionary<string, string> Claims { get; }
    }
}

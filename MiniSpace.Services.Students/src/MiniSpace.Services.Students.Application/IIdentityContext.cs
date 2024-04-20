namespace MiniSpace.Services.Students.Application
{
    public interface IIdentityContext
    {
        Guid Id { get; }
        string Role { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
        bool IsBanned { get; }
        bool IsOrganizer { get; }
        IDictionary<string, string> Claims { get; }
    }    
}

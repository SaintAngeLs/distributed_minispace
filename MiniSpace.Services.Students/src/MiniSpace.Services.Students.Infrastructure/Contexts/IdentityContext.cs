using MiniSpace.Services.Students.Application;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MiniSpace.Services.Students.Application.UnitTests")]


namespace MiniSpace.Services.Students.Infrastructure.Contexts
{
    internal class IdentityContext : IIdentityContext
    {
        public Guid Id { get; }
        public string Role { get; } = string.Empty;
        public string Name { get; } = string.Empty;
        public string Email { get; } = string.Empty;
        public bool IsAuthenticated { get; }
        public bool IsAdmin { get; }
        public bool IsBanned { get; }
        public bool IsOrganizer { get; }
        public IDictionary<string, string> Claims { get; } = new Dictionary<string, string>();

        internal IdentityContext()
        {
        }

        internal IdentityContext(CorrelationContext.UserContext context)
            : this(context.Id, context.Role, context.IsAuthenticated, context.Claims)
        {
        }

        internal IdentityContext(string id, string role, bool isAuthenticated, IDictionary<string, string> claims)
        {
            Id = Guid.TryParse(id, out var userId) ? userId : Guid.Empty;
            Role = role ?? string.Empty;
            IsAuthenticated = isAuthenticated;
            IsAdmin = Role.Equals("admin", StringComparison.InvariantCultureIgnoreCase);
            IsBanned = Role.Equals("banned", StringComparison.InvariantCultureIgnoreCase);
            IsOrganizer = Role.Equals("organizer", StringComparison.InvariantCultureIgnoreCase);
            Claims = claims ?? new Dictionary<string, string>();
            Name = Claims.TryGetValue("name", out var name) ? name : string.Empty;
            Email = Claims.TryGetValue("email", out var email) ? email : string.Empty;
        }
        
        internal static IIdentityContext Empty => new IdentityContext();
    }
}

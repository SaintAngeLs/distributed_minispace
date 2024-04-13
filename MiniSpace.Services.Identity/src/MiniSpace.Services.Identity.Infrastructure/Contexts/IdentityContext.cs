using System;
using System.Collections.Generic;
using MiniSpace.Services.Identity.Application;

namespace MiniSpace.Services.Identity.Infrastructure.Contexts
{
    internal class IdentityContext : IIdentityContext
    {
        public Guid Id { get; }
        public string Role { get; } = string.Empty;
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
            Claims = claims ?? new Dictionary<string, string>();
            Email = Claims.TryGetValue("email", out var email) ? email : string.Empty;
            IsOrganizer = Claims["permissions"] == "organize_events";
        }
        
        internal static IIdentityContext Empty => new IdentityContext();
    }
}
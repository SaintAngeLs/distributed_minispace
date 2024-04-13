using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Identity.Application
{
    public interface IIdentityContext
    {
        Guid Id { get; }
        string Role { get; }
        string Email { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
        bool IsBanned { get; }
        bool IsOrganizer { get; }
        IDictionary<string, string> Claims { get; }
    }
}
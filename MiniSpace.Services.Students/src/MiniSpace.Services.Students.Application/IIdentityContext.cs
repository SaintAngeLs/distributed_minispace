using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application
{
    public interface IIdentityContext
    {
        Guid Id { get; }
        string Role { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
        IDictionary<string, string> Claims { get; }
    }    
}

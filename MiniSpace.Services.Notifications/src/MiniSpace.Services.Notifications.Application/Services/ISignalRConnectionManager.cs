using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Application.Services
{
    public interface ISignalRConnectionManager
    {
        Task SendMessageAsync(string user, string message);
    }
}
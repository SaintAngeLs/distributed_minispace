using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Services.Clients
{
    public interface IFriendsServiceClient
    {
        Task<IEnumerable<UserFriendsDto>> GetAsync(Guid studentId);
    }
}
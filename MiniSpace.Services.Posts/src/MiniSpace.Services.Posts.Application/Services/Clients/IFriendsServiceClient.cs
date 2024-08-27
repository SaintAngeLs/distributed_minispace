using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Posts.Application.Dto;

namespace MiniSpace.Services.Posts.Application.Services.Clients
{
    public interface IFriendsServiceClient
    {
        Task<IEnumerable<UserFriendsDto>> GetAsync(Guid userId);
    }
}
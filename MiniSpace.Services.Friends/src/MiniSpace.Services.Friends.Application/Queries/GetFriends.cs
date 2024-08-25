using Convey.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Core.Wrappers;
using System.Collections.Generic;

namespace MiniSpace.Services.Friends.Application.Queries
{
    public class GetFriends : IQuery<PagedResponse<UserFriendsDto>>
    {
        public Guid UserId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public GetFriends(Guid userId, int page = 1, int pageSize = 10)
        {
            UserId = userId;
            Page = page;
            PageSize = pageSize;
        }
    }  
}
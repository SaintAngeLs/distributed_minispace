using Paralax.CQRS.Queries;
using MiniSpace.Services.Communication.Application.Dto;
using System;
using MiniSpace.Services.Communication.Core.Wrappers;

namespace MiniSpace.Services.Communication.Application.Queries
{
    public class GetUserChats : IQuery<PagedResponse<UserChatDto>>
    {
        public Guid UserId { get; }
        public int Page { get; }
        public int PageSize { get; }

        public GetUserChats(Guid userId, int page, int pageSize)
        {
            UserId = userId;
            Page = page > 0 ? page : 1;
            PageSize = pageSize > 0 ? pageSize : 10;
        }
    }
}

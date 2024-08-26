using Convey.CQRS.Queries;
using MiniSpace.Services.Communication.Application.Dto;
using System;
using System.Collections.Generic;
using MiniSpace.Services.Communication.Core.Wrappers;

namespace MiniSpace.Services.Communication.Application.Queries
{
    public class GetUserChats : IQuery<PagedResponse<UserChatDto>>
    {
        public Guid UserId { get; }

        public GetUserChats(Guid userId)
        {
            UserId = userId;
        }
    }
}

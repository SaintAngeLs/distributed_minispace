using Convey.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;
using System;

namespace MiniSpace.Services.Friends.Application.Queries
{
    public class GetSentFriendRequests : IQuery<IEnumerable<StudentRequestsDto>>
    {
        public Guid StudentId { get; set; }
    }
}

using Paralax.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;

namespace MiniSpace.Services.Friends.Application.Queries
{
    public class GetFriendEvents : IQuery<FriendEventsDto>
    {
        public Guid FriendId { get; set; }
    }
}
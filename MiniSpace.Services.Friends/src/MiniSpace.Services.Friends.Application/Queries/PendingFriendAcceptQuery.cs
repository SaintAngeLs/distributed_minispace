using Convey.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;

namespace MiniSpace.Services.Friends.Application.Queries
{
    public class PendingFriendAcceptQuery : IQuery<FriendRequestDto>
    {
        public Guid FriendRequestId { get; }

        public PendingFriendAcceptQuery(Guid friendRequestId)
        {
            FriendRequestId = friendRequestId;
        }
    }
}

using Convey.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;

namespace MiniSpace.Services.Friends.Application.Queries
{
    public class PendingFriendDeclineQuery : IQuery<FriendRequestDto>
    {
        public Guid FriendRequestId { get; }

        public PendingFriendDeclineQuery(Guid friendRequestId)
        {
            FriendRequestId = friendRequestId;
        }
    }
}

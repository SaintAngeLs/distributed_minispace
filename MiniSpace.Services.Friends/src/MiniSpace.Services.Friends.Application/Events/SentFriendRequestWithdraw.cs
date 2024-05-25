using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Friends.Application.Events
{
   public class SentFriendRequestWithdrawHandler : IEvent
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public SentFriendRequestWithdrawHandler(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}


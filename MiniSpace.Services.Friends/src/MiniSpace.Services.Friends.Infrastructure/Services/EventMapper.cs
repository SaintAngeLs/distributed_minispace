using Convey.CQRS.Events;
using MiniSpace.Services.Friends.Application.Events;
using MiniSpace.Services.Friends.Application.Events.External;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Events;

namespace MiniSpace.Services.Friends.Infrastructure.Services
{
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);

        public IEvent Map(IDomainEvent @event)
        {
            switch (@event)
            {
                
                case Core.Events.FriendRemoved e:
                    return new Application.Events.FriendRemoved(e.Requester.Id, e.Friend.Id);

                // case Core.Events.FriendshipConfirmed e:
                //     return new PendingFriendAccepted(e.FriendId, e.FriendId);

                case Core.Events.FriendshipDeclined e:
                    return new PendingFriendDeclined(e.RequesterId, e.FriendId);
                
                case  Core.Events.FriendInvited e:
                    return new Application.Events.External.FriendInvited(e.Inviter.Id, e.Invitee.Id);

                case FriendRequest e when !(e is Core.Events.FriendInvited):
                    return new Application.Events.External.FriendRequestCreated(e.InviterId, e.InviteeId);

                case Core.Events.PendingFriendAccepted e:
                    return new Application.Events.External.PendingFriendAccepted(e.RequesterId, e.FriendId);

                default:
                    return null; 
            }
        }

        public IEnumerable<IEvent> MapAll(Core.Events.PendingFriendAccepted pendingFriendAccept)
        {
            // Implementation for the new method
            return new List<IEvent> { new Application.Events.External.PendingFriendAccepted(pendingFriendAccept.RequesterId, pendingFriendAccept.FriendId) };
        }
    }
}

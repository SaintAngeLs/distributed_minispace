using Convey.Logging.CQRS;
using MiniSpace.Services.Friends.Application.Commands;
using MiniSpace.Services.Friends.Application.Events;
using MiniSpace.Services.Friends.Application.Events.External;

namespace MiniSpace.Services.Friends.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(AddFriend), new HandlerLogTemplate
                    {
                        After = "Friendship added between requester: {RequesterId} and friend: {FriendId}."
                    }
                },
                {
                    typeof(RemoveFriend), new HandlerLogTemplate
                    {
                        After = "Friendship removed between requester: {RequesterId} and friend: {FriendId}."
                    }
                },
                {
                    typeof(InviteFriend), new HandlerLogTemplate
                    {
                        After = "Friendship invitation sent from inviter: {InviterId} to invitee: {InviteeId}."
                    }
                },
                {
                    typeof(PendingFriendAccept), new HandlerLogTemplate
                    {
                        After = "Friendship acceptance processed for requester: {RequesterId} with friend: {FriendId}."
                    }
                },
                {
                    typeof(PendingFriendDecline), new HandlerLogTemplate
                    {
                        After = "Friendship decline processed for requester: {RequesterId} with friend: {FriendId}."
                    }
                },
                {
                    typeof(FriendRequestSent), new HandlerLogTemplate
                    {
                        After = "Friend request sent from: {InviterId} to: {InviteeId}."
                    }
                },
                { 
                    typeof(FriendInvited), new HandlerLogTemplate 
                    { 
                        After = "Friend invited by: {InviterId} to {InviteeId}. Invitation created at: {CreatedAt}." 
                    } 
                },
                { 
                    typeof(FriendRequestCreated), new HandlerLogTemplate 
                    { 
                        After = "Friend request created between requester: {RequesterId} and friend: {FriendId}." 
                    } 
                }
            };
        
        public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var key = message.GetType();
            return MessageTemplates.TryGetValue(key, out var template) ? template : null;
        }
    }
}

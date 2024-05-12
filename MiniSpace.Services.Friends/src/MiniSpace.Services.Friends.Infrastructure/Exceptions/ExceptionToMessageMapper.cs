using Convey.MessageBrokers.RabbitMQ;
using MiniSpace.Services.Friends.Application.Commands;
using MiniSpace.Services.Friends.Application.Events.Rejected;
using MiniSpace.Services.Friends.Application.Events.External;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Core.Exceptions;

namespace MiniSpace.Services.Friends.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
            {
                AlreadyFriendsException ex => new FriendAddingFailed(ex.RequesterId, ex.FriendId, ex.Message, "already_friends"),
                FriendshipNotFoundException ex => message switch
                {
                    AddFriend _ => new FriendAddingFailed(ex.RequesterId, ex.FriendId, ex.Message, "friendship_not_found"),
                    RemoveFriend _ => new FriendRemovalFailed(ex.RequesterId, ex.FriendId, ex.Message, "friendship_not_found"),
                    _ => null
                },
                UnauthorizedFriendActionException ex => new FriendAddingFailed(ex.RequesterId, ex.FriendId, ex.Message, "unauthorized_action"),
                _ => null
            };

    }
}

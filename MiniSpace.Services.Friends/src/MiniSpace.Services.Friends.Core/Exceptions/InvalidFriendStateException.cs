using System;

namespace MiniSpace.Services.Friends.Core.Exceptions
{
    public class InvalidFriendStateException : Exception
    {
        public Guid FriendId { get; }

        public InvalidFriendStateException(Guid friendId, string message) : base(message)
        {
            FriendId = friendId;
        }
    }
}

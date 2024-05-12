using System;

namespace MiniSpace.Services.Friends.Core.Exceptions
{
    public class InvalidFriendFullNameException : Exception
    {
        public Guid FriendId { get; }
        public string FullName { get; }

        public InvalidFriendFullNameException(Guid friendId, string fullName)
            : base($"Invalid full name '{fullName}' provided for friend with ID {friendId}.")
        {
            FriendId = friendId;
            FullName = fullName;
        }
    }
}

using System;

namespace MiniSpace.Services.Students.Application.Exceptions
{
    public class UserNotBlockedException : AppException
    {
        public override string Code { get; } = "user_not_blocked";

        public UserNotBlockedException(Guid blockerId, Guid blockedUserId)
            : base($"User with ID '{blockedUserId}' is not blocked by user with ID '{blockerId}'.")
        {
        }
    }
}

namespace MiniSpace.Services.Students.Application.Exceptions
{
    public class UserAlreadyBlockedException : AppException
    {
        public override string Code { get; } = "user_already_blocked";

        public UserAlreadyBlockedException(Guid blockerId, Guid blockedUserId)
            : base($"User with ID: '{blockedUserId}' is already blocked by user with ID: '{blockerId}'.")
        {
        }
    }
}

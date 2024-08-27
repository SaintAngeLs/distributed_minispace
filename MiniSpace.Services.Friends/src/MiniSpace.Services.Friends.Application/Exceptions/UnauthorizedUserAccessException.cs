namespace MiniSpace.Services.Friends.Application.Exceptions
{
    public class UnauthorizedUserAccessException : AppException
    {
        public override string Code { get; } = "unauthorized_user_access";
        public Guid UserId { get; }
        public Guid AccessUserId { get; }

        public UnauthorizedUserAccessException(Guid userId, Guid accessUserId)
            : base($"Unauthorized access to user with id: '{userId}' by access user with id: '{userId}'.")
        {
            UserId = userId;
            AccessUserId = accessUserId;
        }
    }
}

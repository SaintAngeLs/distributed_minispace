using System;

namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class UserNotMemberException : AppException
    {
        public override string Code { get; } = "user_not_member";
        public Guid UserId { get; }
        public Guid OrganizationId { get; }

        public UserNotMemberException(Guid userId, Guid organizationId)
            : base($"User with ID '{userId}' is not a member of organization with ID '{organizationId}'.")
        {
            UserId = userId;
            OrganizationId = organizationId;
        }
    }
}

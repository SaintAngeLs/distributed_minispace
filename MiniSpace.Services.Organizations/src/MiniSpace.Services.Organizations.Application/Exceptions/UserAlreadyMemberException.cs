using System;

namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class UserAlreadyMemberException : AppException
    {
        public override string Code { get; } = "user_already_member";
        public Guid UserId { get; }
        public Guid OrganizationId { get; }

        public UserAlreadyMemberException(Guid userId, Guid organizationId)
            : base($"User with ID: {userId} is already a member of organization with ID: {organizationId}.")
        {
            UserId = userId;
            OrganizationId = organizationId;
        }
    }
}

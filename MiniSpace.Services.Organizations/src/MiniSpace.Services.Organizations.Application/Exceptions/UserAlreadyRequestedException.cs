using System;

namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class UserAlreadyRequestedException : AppException
    {
        public override string Code { get; } = "user_already_requested";
        public Guid UserId { get; }
        public Guid OrganizationId { get; }

        public UserAlreadyRequestedException(Guid userId, Guid organizationId)
            : base($"User with ID: {userId} has already requested access to organization with ID: {organizationId}.")
        {
            UserId = userId;
            OrganizationId = organizationId;
        }
    }
}

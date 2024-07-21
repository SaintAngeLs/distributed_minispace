namespace MiniSpace.Services.Organizations.Core.Exceptions
{
    public class UserAlreadyInvitedException : DomainException
    {
        public override string Code { get; } = "user_already_invited";
        public Guid UserId { get; }
        public Guid OrganizationId { get; }

        public UserAlreadyInvitedException(Guid userId, Guid organizationId) 
            : base($"User with ID: '{userId}' has already been invited to organization with ID: '{organizationId}'.")
        {
            UserId = userId;
            OrganizationId = organizationId;
        }
    }
}

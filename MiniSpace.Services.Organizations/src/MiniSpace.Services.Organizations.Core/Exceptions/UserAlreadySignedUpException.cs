namespace MiniSpace.Services.Organizations.Core.Exceptions
{
    public class UserAlreadySignedUpException : DomainException
    {
        public override string Code { get; } = "user_already_signed_up";
        public Guid UserId { get; }
        public Guid OrganizationId { get; }

        public UserAlreadySignedUpException(Guid userId, Guid organizationId) 
            : base($"User with ID: '{userId}' has already signed up for organization with ID: '{organizationId}'.")
        {
            UserId = userId;
            OrganizationId = organizationId;
        }
    }
}

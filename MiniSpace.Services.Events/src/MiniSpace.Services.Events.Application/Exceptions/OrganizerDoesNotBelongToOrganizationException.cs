using System;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    public class OrganizerDoesNotBelongToOrganizationException : AppException
    {
        public override string Code => "organizer_does_not_belong_to_organization";
        public Guid OrganizerId { get; }
        public Guid OrganizationId { get; }

        public OrganizerDoesNotBelongToOrganizationException(Guid organizerId, Guid organizationId) 
            : base($"Organizer with ID: {organizerId} does not belong to organization with ID: {organizationId}.")
        {
            OrganizerId = organizerId;
            OrganizationId = organizationId;
        }
    }
}
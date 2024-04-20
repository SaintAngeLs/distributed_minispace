using System;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    public class OrganizerCannotAddEventForAnotherOrganizerException : AppException
    {
        public override string Code { get; } = "organizer_cannot_add_event_for_another_organizer";
        public Guid OrganizerId { get; }
        public Guid RequestedOrganizerId { get; }

        public OrganizerCannotAddEventForAnotherOrganizerException(Guid organizerId, Guid requestedOrganizerId)
            : base($"Organizer with ID: {organizerId} cannot add event for organizer with ID: {requestedOrganizerId}.")
        {
            OrganizerId = organizerId;
            RequestedOrganizerId = requestedOrganizerId;
        }
    }
}
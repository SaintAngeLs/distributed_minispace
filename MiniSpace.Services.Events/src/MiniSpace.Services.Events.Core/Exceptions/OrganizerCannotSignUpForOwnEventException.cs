using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class OrganizerCannotSignUpForOwnEventException : DomainException
    {
        public override string Code { get; } = "organizer_cannot_sign_up_for_own_event";
        public Guid OrganizerId { get; }
        public Guid EventId { get; }
        public OrganizerCannotSignUpForOwnEventException(Guid organizerId, Guid eventId) 
            : base($"Organizer with id: {organizerId} cannot sign up for their own event with id: {eventId}.")
        {
            OrganizerId = organizerId;
            EventId = eventId;
        }
    }
}
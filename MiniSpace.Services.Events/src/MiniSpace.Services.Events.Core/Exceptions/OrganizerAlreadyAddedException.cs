using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class OrganizerAlreadyAddedException : DomainException
    {
        public override string Code { get; } = "organizer_already_added";

        public OrganizerAlreadyAddedException(Guid id)
            : base($"Organizer with id: '{id}' has been already added")
        {
        }
    }
}
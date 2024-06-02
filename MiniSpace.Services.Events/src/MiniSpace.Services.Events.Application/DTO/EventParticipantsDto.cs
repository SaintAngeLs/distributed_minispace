using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class EventParticipantsDto
    {
        public Guid EventId { get; set; }
        public IEnumerable<ParticipantDto> InterestedStudents { get; set; }
        public IEnumerable<ParticipantDto> SignedUpStudents { get; set; }
    }
}
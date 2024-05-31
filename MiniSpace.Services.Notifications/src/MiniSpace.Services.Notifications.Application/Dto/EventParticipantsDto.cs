using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Notifications.Application.Dto
{
    public class EventParticipantsDto
    {
        public Guid EventId { get; set; }
        public IEnumerable<ParticipantDto> InterestedStudents { get; set; }
        public IEnumerable<ParticipantDto> SignedUpStudents { get; set; }
    }
}
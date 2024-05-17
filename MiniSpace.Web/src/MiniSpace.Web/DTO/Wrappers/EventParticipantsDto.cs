using System;
using System.Collections.Generic;

namespace MiniSpace.Web.DTO.Wrappers
{
    public class EventParticipantsDto
    {
        public Guid EventId { get; set; }
        public IEnumerable<ParticipantDto> InterestedStudents { get; set; }
        public IEnumerable<ParticipantDto> SignedUpStudents { get; set; }
    }
}
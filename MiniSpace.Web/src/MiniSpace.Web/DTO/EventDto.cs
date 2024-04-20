using System;
using System.Collections.Generic;

namespace MiniSpace.Web.DTO
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OrganizerDto Organizer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<OrganizerDto> CoOrganizers { get; set; }
        public AddressDto Location { get; set; }
        public int InterestedStudents { get; set; }
        public int SignedUpStudents { get; set; }
        public int Capacity { get; set; }
        public decimal Fee { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.DTO
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
        //public string Image { get; set; }
        public int InterestedStudents { get; set; }
        public int SignedUpStudents { get; set; }
        public int Capacity { get; set; }
        public decimal Fee { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        
        public EventDto()
        {
        }

        public EventDto(Event @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            Description = @event.Description;
            Organizer = new OrganizerDto(@event.Organizer);
            StartDate = @event.StartDate;
            EndDate = @event.EndDate;
            CoOrganizers = @event.CoOrganizers.Select(x => new OrganizerDto(x));
            Location = new AddressDto(@event.Location);
            InterestedStudents = @event.InterestedStudents.Count();
            SignedUpStudents = @event.SignedUpStudents.Count();
            Capacity = @event.Capacity;
            Fee = @event.Fee;
            Category = @event.Category.ToString();
            Status = @event.State.ToString();
        }
    }
}
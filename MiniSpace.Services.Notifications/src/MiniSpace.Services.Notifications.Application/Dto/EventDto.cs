using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Notifications.Application.DTO;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Application.Dto
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OrganizerDto Organizer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        // public AddressDto Location { get; set; }
        public IEnumerable<Guid> MediaFiles { get; set; }
        public int InterestedStudents { get; set; }
        public int SignedUpStudents { get; set; }
        public int Capacity { get; set; }
        public decimal Fee { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsSignedUp { get; set; }
        public bool IsInterested { get; set; }
        public int? StudentRating { get; set; }
        public IEnumerable<ParticipantDto> FriendsInterestedIn { get; set; }
        public IEnumerable<ParticipantDto> FriendsSignedUp { get; set; }
        
        public EventDto()
        {
        }

        // public EventDto(Event @event, Guid studentId)
        // {
        //     Id = @event.Id;
        //     Name = @event.Name;
        //     Description = @event.Description;
        //     Organizer = new OrganizerDto(@event.Organizer);
        //     StartDate = @event.StartDate;
        //     EndDate = @event.EndDate;
        //     // Location = new AddressDto(@event.Location);
        //     MediaFiles = @event.MediaFiles;
        //     InterestedStudents = @event.InterestedStudents.Count();
        //     SignedUpStudents = @event.SignedUpStudents.Count();
        //     Capacity = @event.Capacity;
        //     Fee = @event.Fee;
        //     Category = @event.Category.ToString();
        //     Status = @event.State.ToString();
        //     PublishDate = @event.PublishDate;
        //     IsSignedUp = @event.SignedUpStudents.Any(x => x.StudentId == studentId);
        //     IsInterested = @event.InterestedStudents.Any(x => x.StudentId == studentId);
        //     StudentRating = @event.Ratings.FirstOrDefault(x => x.StudentId == studentId)?.Value;
        //     FriendsInterestedIn = Enumerable.Empty<ParticipantDto>();
        //     FriendsSignedUp = Enumerable.Empty<ParticipantDto>();
        // }
    }
}
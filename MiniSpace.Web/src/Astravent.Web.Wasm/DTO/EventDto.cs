using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.Areas.Events.CommandsDto;
using Astravent.Web.Wasm.DTO.Events;

namespace Astravent.Web.Wasm.DTO
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OrganizerDto Organizer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AddressDto Location { get; set; }
        public IEnumerable<string> MediaFilesUrl { get; set; }
        public int InterestedStudents { get; set; }
        public int SignedUpStudents { get; set; }
        public int Capacity { get; set; }
        public decimal Fee { get; set; }
        public Category Category { get; set; }
        public string Status { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsSignedUp { get; set; }
        public bool IsInterested { get; set; }
        public int? StudentRating { get; set; }
        public IEnumerable<ParticipantDto> FriendsInterestedIn { get; set; }
        public IEnumerable<ParticipantDto> FriendsSignedUp { get; set; }
        public string BannerUrl { get; set; }
        public EventSettings Settings { get; set; }
        public EventDto() { }
    }
}
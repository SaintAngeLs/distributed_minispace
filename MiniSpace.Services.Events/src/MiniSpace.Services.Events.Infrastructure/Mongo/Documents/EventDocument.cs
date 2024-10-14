using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Paralax.Types;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class EventDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OrganizerDocument Organizer { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AddressDocument Location { get; set; }
        public IList<string> MediaFiles { get; set; } 
        public string BannerUrl { get; set; }
        public int Capacity { get; set; }
        public decimal Fee { get; set; }
        public Category Category { get; set; }
        public State State { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Visibility Visibility { get; set; }
        public EventSettingsDocument Settings { get; set; }
        public IEnumerable<ParticipantDocument> InterestedStudents { get; set; } 
        public IEnumerable<ParticipantDocument> SignedUpStudents { get; set; }
        public IEnumerable<RatingDocument> Ratings { get; set; }
    }
}
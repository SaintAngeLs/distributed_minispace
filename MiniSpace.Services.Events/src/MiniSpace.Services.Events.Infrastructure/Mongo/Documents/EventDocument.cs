using System;
using System.Collections;
using System.Collections.Generic;
using Convey.Types;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    public class EventDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<Organizer> Organizers { get; set; }
        public Address Location { get; set; }
        //public string Image { get; set; }
        public IEnumerable<Student> InterestedStudents { get; set; }
        public IEnumerable<Student> SignedUpStudents { get; set; }
        public int Capacity { get; set; }
        public decimal Fee { get; set; }
        public Category Category { get; set; }
        public State State { get; set; }
        public DateTime PublishDate { get; set; }
        public IEnumerable<Reaction> Reactions { get; set; }
        public IEnumerable<Rating> Ratings { get; set; }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Events.Core.Exceptions;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Event: AggregateRoot
    {
        private ISet<Organizer> _organizers = new HashSet<Organizer>();
        private ISet<Student> _interestedStudents = new HashSet<Student>();
        private ISet<Student> _signedUpStudents = new HashSet<Student>();
        private ISet<Reaction> _reactions = new HashSet<Reaction>();
        private ISet<Rating> _ratings = new HashSet<Rating>();
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public Address Location { get; private set; }
        //public string Image { get; set; }
        public int Capacity { get; private set; }
        public decimal Fee { get; private set; }
        public Category Category { get; private set; }
        public Status Status { get; private set; }
        public DateTime PublishDate { get; private set; }
        
        public IEnumerable<Organizer> Organizers
        {
            get => _organizers;
            private set => _organizers = new HashSet<Organizer>(value);
        }
        
        public IEnumerable<Student> InterestedStudents
        {
            get => _interestedStudents;
            private set => _interestedStudents = new HashSet<Student>(value);
        }
        
        public IEnumerable<Student> SignedUpStudents
        {
            get => _signedUpStudents;
            private set => _signedUpStudents = new HashSet<Student>(value);
        }
        
        public IEnumerable<Reaction> Reactions
        {
            get => _reactions;
            private set => _reactions = new HashSet<Reaction>(value);
        }
        
        public IEnumerable<Rating> Ratings
        {
            get => _ratings;
            private set => _ratings = new HashSet<Rating>(value);
        }

        public Event(AggregateId id,  string name, string description, DateTime startDate, DateTime endDate, 
            Address location, int capacity, decimal fee, Category category, Status status, DateTime publishDate, 
            IEnumerable<Organizer> organizers = null, IEnumerable<Student> interestedStudents = null, 
            IEnumerable<Student> signedUpStudents = null, IEnumerable<Reaction> reactions = null,
            IEnumerable<Rating> ratings = null)
        {
            Id = id;
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Location = location;
            Capacity = capacity;
            Fee = fee;
            Category = category;
            Status = status;
            Organizers = organizers ?? Enumerable.Empty<Organizer>();
            InterestedStudents = interestedStudents ?? Enumerable.Empty<Student>();
            SignedUpStudents = signedUpStudents ?? Enumerable.Empty<Student>();
            Reactions = reactions ?? Enumerable.Empty<Reaction>();
            Ratings = ratings ?? Enumerable.Empty<Rating>();
            PublishDate = publishDate;
        }
        
        public static Event Create(AggregateId id,  string name, string description, DateTime startDate, DateTime endDate, 
            Address location, int capacity, decimal fee, Category category, Status status, DateTime publishDate, Guid organizerId)
        {
            var organizer = new Organizer(organizerId, "", "", "");
            var @event = new Event(id, name, description, startDate, endDate, location, capacity, fee, category, status, publishDate);
            @event.AddOrganizer(organizer);
            return @event;
        }
        
        public void AddOrganizer(Organizer organizer)
        {
            if (Organizers.Any(o => o.Id == organizer.Id))
            {
                throw new OrganizerAlreadyAddedException(organizer.Id);
            }

            _organizers.Add(organizer);
        }
        
        public void SignUpStudent(Student student)
        {
            if (SignedUpStudents.Any(s => s.Id == student.Id))
            {
                throw new StudentAlreadySignedUpException(student.Id, Id);
            }

            if (SignedUpStudents.Count() >= Capacity)
            {
                throw new EventCapacityExceededException(Id, Capacity);
            }

            _signedUpStudents.Add(student);
        }
        
        public void ShowStudentInterest(Student student)
        {
            if (InterestedStudents.Any(s => s.Id == student.Id))
            {
                throw new StudentAlreadyInterestedInEventException(student.Id, Id);
            }

            _interestedStudents.Add(student);
        }
    }
}
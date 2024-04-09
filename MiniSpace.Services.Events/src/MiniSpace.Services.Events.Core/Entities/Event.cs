using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Event: AggregateRoot
    {
        private ISet<Organizer> _organizers = new HashSet<Organizer>();
        private ISet<Student> _interestedStudents = new HashSet<Student>();
        private ISet<Student> _registeredStudents = new HashSet<Student>();
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
        
        public IEnumerable<Student> RegisteredStudents
        {
            get => _registeredStudents;
            private set => _registeredStudents = new HashSet<Student>(value);
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
            Address location, int capacity, decimal fee, Category category, Status status)
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
        }
    }
}
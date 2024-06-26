﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Events.Core.Exceptions;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Event: AggregateRoot
    {
        private ISet<Participant> _interestedStudents = new HashSet<Participant>();
        private ISet<Participant> _signedUpStudents = new HashSet<Participant>();
        private ISet<Rating> _ratings = new HashSet<Rating>();
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Organizer Organizer { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public Address Location { get; private set; }
        public IEnumerable<Guid> MediaFiles { get; set; }
        public int Capacity { get; private set; }
        public decimal Fee { get; private set; }
        public Category Category { get; private set; }
        public State State { get; private set; }
        public DateTime PublishDate { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        
        public IEnumerable<Participant> InterestedStudents
        {
            get => _interestedStudents;
            private set => _interestedStudents = new HashSet<Participant>(value);
        }
        
        public IEnumerable<Participant> SignedUpStudents
        {
            get => _signedUpStudents;
            private set => _signedUpStudents = new HashSet<Participant>(value);
        }
        
        public IEnumerable<Rating> Ratings
        {
            get => _ratings;
            private set => _ratings = new HashSet<Rating>(value);
        }

        public Event(AggregateId id,  string name, string description, DateTime startDate, DateTime endDate, 
            Address location, IEnumerable<Guid> mediaFiles, int capacity, decimal fee, Category category, State state, 
            DateTime publishDate, Organizer organizer, DateTime updatedAt, IEnumerable<Participant> interestedStudents = null, 
            IEnumerable<Participant> signedUpStudents = null, IEnumerable<Rating> ratings = null)
        {
            Id = id;
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Location = location;
            MediaFiles = mediaFiles;
            Capacity = capacity;
            Fee = fee;
            Category = category;
            State = state;
            Organizer = organizer;
            InterestedStudents = interestedStudents ?? Enumerable.Empty<Participant>();
            SignedUpStudents = signedUpStudents ?? Enumerable.Empty<Participant>();
            Ratings = ratings ?? Enumerable.Empty<Rating>();
            PublishDate = publishDate;
            UpdatedAt = updatedAt;
        }
        
        public static Event Create(AggregateId id,  string name, string description, DateTime startDate, DateTime endDate, 
            Address location, IEnumerable<Guid> mediaFiles, int capacity, decimal fee, Category category, State state,
            DateTime publishDate, Organizer organizer, DateTime now)
        {
            var @event = new Event(id, name, description, startDate, endDate, location, mediaFiles, capacity, fee, 
                category, state, publishDate, organizer, now);
            return @event;
        }
        
        public void Update(string name, string description, DateTime startDate, DateTime endDate, Address location,
            int capacity, decimal fee, Category category, State state, DateTime publishDate, DateTime now)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Location = location;
            Capacity = capacity;
            Fee = fee;
            Category = category;
            State = state;
            PublishDate = publishDate;
            UpdatedAt = now;
        }
        
        public void SignUpStudent(Participant participant)
        {
            if(State != State.Published)
            {
                throw new InvalidEventState(Id, State.Published, State);
            }
            AddParticipant(participant);
        }
        
        public void AddParticipant(Participant participant)
        {
            if (SignedUpStudents.Any(p => p.StudentId == participant.StudentId))
            {
                throw new StudentAlreadySignedUpException(participant.StudentId, Id);
            }

            if (SignedUpStudents.Count() >= Capacity)
            {
                throw new EventCapacityExceededException(Id, Capacity);
            }
            
            if(participant.StudentId == Organizer.Id)
            {
                throw new OrganizerCannotSignUpForOwnEventException(Organizer.Id, Id);
            }

            _signedUpStudents.Add(participant);
        }
        
        public void CancelSignUp(Guid studentId)
        {
            if(State != State.Published)
            {
                throw new InvalidEventState(Id, State.Published, State);
            }
            RemoveParticipant(studentId);
        }
        
        public void RemoveParticipant(Guid studentId)
        {
            var participant = _signedUpStudents.SingleOrDefault(p => p.StudentId == studentId);
            if (participant is null)
            {
                throw new StudentNotSignedUpException(studentId, Id);
            }

            _signedUpStudents.Remove(participant);
        }
        
        public void ShowStudentInterest(Participant participant)
        {
            if (InterestedStudents.Any(p => p.StudentId == participant.StudentId))
            {
                throw new StudentAlreadyInterestedInEventException(participant.StudentId, Id);
            }

            _interestedStudents.Add(participant);
        }
        
        public void CancelInterest(Guid studentId)
        {
            var participant = _interestedStudents.SingleOrDefault(p => p.StudentId == studentId);
            if (participant is null)
            {
                throw new StudentNotInterestedInEventException(studentId, Id);
            }

            _interestedStudents.Remove(participant);
        }
        
        public void Rate(Guid studentId, int rating)
        {
            if(State != State.Archived)
            {
                throw new InvalidEventState(Id, State.Archived, State);
            }
            
            if(_signedUpStudents.All(p => p.StudentId != studentId))
            {
                throw new StudentNotSignedUpForEventException(Id ,studentId);
            }
            
            if (rating < 1 || rating > 5)
            {
                throw new InvalidRatingValueException(rating);
            }
            
            if (_ratings.Any(r => r.StudentId == studentId))
            {
                throw new StudentAlreadyRatedException(studentId, Id);
            }

            _ratings.Add(new Rating(studentId, rating));
        }
        
        public void CancelRate(Guid studentId)
        {
            var rating = _ratings.SingleOrDefault(r => r.StudentId == studentId);
            if (rating is null)
            {
                throw new StudentNotRatedEventException(studentId, Id);
            }

            _ratings.Remove(rating);
        }

        public bool UpdateState(DateTime now)
        {
            if(State == State.ToBePublished && PublishDate <= now)
            {
                ChangeState(State.Published);
            }
            else if (State == State.Published && EndDate <= now)
            {
                ChangeState(State.Archived);
            }
            else
                return false;

            UpdatedAt = now;
            return true;
        }
        
        private void ChangeState(State state)
        {
            if (State == state)
            {
                return;
            }

            State = state;
        }
        
        public void RemoveMediaFile(Guid mediaFileId)
        {
            var mediaFile = MediaFiles.SingleOrDefault(mf => mf == mediaFileId);
            if (mediaFile == Guid.Empty)
            {
                throw new MediaFileNotFoundException(mediaFileId, Id);
            }
        }
        
        public bool IsOrganizer(Guid organizerId)
            => Organizer.Id == organizerId;
    }
}   
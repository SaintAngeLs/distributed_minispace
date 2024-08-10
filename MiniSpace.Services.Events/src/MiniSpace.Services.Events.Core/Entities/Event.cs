using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Events.Core.Exceptions;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Event : AggregateRoot
    {
        private ISet<Participant> _interestedParticipants = new HashSet<Participant>();
        private ISet<Participant> _signedUpParticipants = new HashSet<Participant>();
        private ISet<Rating> _ratings = new HashSet<Rating>();

        public string Name { get; private set; }
        public string Description { get; private set; }
        public Organizer Organizer { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public Address Location { get; private set; }
        public IList<string> MediaFiles { get; set; }
        public string BannerUrl { get; private set; } 
        public int Capacity { get; private set; }
        public decimal Fee { get; private set; }
        public Category Category { get; private set; }
        public State State { get; private set; }
        public DateTime PublishDate { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public Visibility Visibility { get; private set; } 
        public EventSettings Settings { get; private set; }

        public IEnumerable<Participant> InterestedParticipants
        {
            get => _interestedParticipants;
            private set => _interestedParticipants = new HashSet<Participant>(value);
        }

        public IEnumerable<Participant> SignedUpParticipants
        {
            get => _signedUpParticipants;
            private set => _signedUpParticipants = new HashSet<Participant>(value);
        }

        public IEnumerable<Rating> Ratings
        {
            get => _ratings;
            private set => _ratings = new HashSet<Rating>(value);
        }

        public Event(AggregateId id, string name, string description, Organizer organizer,
            DateTime startDate, DateTime endDate, Address location, IList<string> mediaFiles, string bannerUrl, 
            int capacity, decimal fee, Category category, State state, DateTime publishDate, DateTime updatedAt, 
            Visibility visibility, EventSettings settings, IEnumerable<Participant> interestedParticipants = null, 
            IEnumerable<Participant> signedUpParticipants = null, IEnumerable<Rating> ratings = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Organizer = organizer;
            StartDate = startDate;
            EndDate = endDate;
            Location = location;
            MediaFiles = mediaFiles;
            BannerUrl = bannerUrl;
            Capacity = capacity;
            Fee = fee;
            Category = category;
            State = state;
            PublishDate = publishDate;
            UpdatedAt = updatedAt;
            Visibility = visibility;
            Settings = settings;
            InterestedParticipants = interestedParticipants ?? Enumerable.Empty<Participant>();
            SignedUpParticipants = signedUpParticipants ?? Enumerable.Empty<Participant>();
            Ratings = ratings ?? Enumerable.Empty<Rating>();
        }

        public static Event Create(AggregateId id, string name, string description, Organizer organizer,
            DateTime startDate, DateTime endDate, Address location, IList<string> mediaFiles, string bannerUrl, 
            int capacity, decimal fee, Category category, State state, DateTime publishDate, DateTime now, 
            Visibility visibility, EventSettings settings)
        {
            return new Event(id, name, description, organizer, startDate, endDate, location, mediaFiles, 
                bannerUrl, capacity, fee, category, state, publishDate, now, visibility, settings);
        }

        public void Update(string name, string description, DateTime startDate, DateTime endDate, Address location,
            IList<string> mediaFiles, string bannerUrl, int capacity, decimal fee, Category category, State state, 
            DateTime publishDate, DateTime now, Visibility visibility, EventSettings settings)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Location = location;
            MediaFiles = mediaFiles;
            BannerUrl = bannerUrl;
            Capacity = capacity;
            Fee = fee;
            Category = category;
            State = state;
            PublishDate = publishDate;
            UpdatedAt = now;
            Visibility = visibility;
            Settings = settings;
        }

        public void SignUpParticipant(Participant participant)
        {
            if (State != State.Published)
            {
                throw new InvalidEventState(Id, State.Published, State);
            }
            AddParticipant(participant);
        }

        public void AddParticipant(Participant participant)
        {
            if (SignedUpParticipants.Any(p => p.StudentId == participant.StudentId))
            {
                throw new StudentAlreadySignedUpException(participant.StudentId, Id);
            }

            if (SignedUpParticipants.Count() >= Capacity)
            {
                throw new EventCapacityExceededException(Id, Capacity);
            }
            // Theoretically here the assumption is that no matter if the user is organize or not, 
            // they may not sign-up or show interest to evnet, so theoretically, I should be able to 
            // add user who was the organizer to the event.
            // if (participant.StudentId == Organizer.UserId && Organizer.OrganizerType == OrganizerType.User)
            // {
            //     throw new OrganizerCannotSignUpForOwnEventException(Organizer.UserId.Value, Id);
            // }

            _signedUpParticipants.Add(participant);
        }

        public void CancelSignUp(Guid studentId)
        {
            if (State != State.Published)
            {
                throw new InvalidEventState(Id, State.Published, State);
            }
            RemoveParticipant(studentId);
        }

        public void RemoveParticipant(Guid studentId)
        {
            var participant = _signedUpParticipants.SingleOrDefault(p => p.StudentId == studentId);
            if (participant is null)
            {
                throw new StudentNotSignedUpException(studentId, Id);
            }

            _signedUpParticipants.Remove(participant);
        }

        public void UpdateBannerUrl(string newBannerUrl)
        {
            if (string.IsNullOrWhiteSpace(newBannerUrl))
            {
                throw new InvalidBannerUrlException(newBannerUrl);
            }

            BannerUrl = newBannerUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddGalleryImage(string newImageUrl)
        {
            if (string.IsNullOrWhiteSpace(newImageUrl))
            {
                throw new InvalidGalleryImageUrlException(newImageUrl);
            }

            MediaFiles.Add(newImageUrl);
            UpdatedAt = DateTime.UtcNow;
        }

        public void ShowParticipantInterest(Participant participant)
        {
            if (InterestedParticipants.Any(p => p.StudentId == participant.StudentId))
            {
                throw new StudentAlreadyInterestedInEventException(participant.StudentId, Id);
            }

            _interestedParticipants.Add(participant);
        }

        public void CancelInterest(Guid studentId)
        {
            var participant = _interestedParticipants.SingleOrDefault(p => p.StudentId == studentId);
            if (participant is null)
            {
                throw new StudentNotInterestedInEventException(studentId, Id);
            }

            _interestedParticipants.Remove(participant);
        }

        public void Rate(Guid studentId, int rating)
        {
            if (State != State.Archived)
            {
                throw new InvalidEventState(Id, State.Archived, State);
            }

            if (_signedUpParticipants.All(p => p.StudentId != studentId))
            {
                throw new StudentNotSignedUpForEventException(Id, studentId);
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
            if (State == State.ToBePublished && PublishDate <= now)
            {
                ChangeState(State.Published);
            }
            else if (State == State.Published && EndDate <= now)
            {
                ChangeState(State.Archived);
            }
            else
            {
                return false;
            }

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

        public void RemoveMediaFile(string mediaFileUrl)
        {
            if (!MediaFiles.Contains(mediaFileUrl))
            {
                throw new MediaFileNotFoundException(mediaFileUrl, Id);
            }

            MediaFiles.Remove(mediaFileUrl);
        }

        public bool IsOrganizer(Guid organizerId)
            => Organizer.UserId == organizerId && Organizer.OrganizerType == OrganizerType.User;
    }
}

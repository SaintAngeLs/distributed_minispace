using MiniSpace.Services.Students.Core.Events;
using MiniSpace.Services.Students.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class Student : AggregateRoot
    {
        private ISet<Guid> _interestedInEvents = new HashSet<Guid>();
        private ISet<Guid> _signedUpEvents = new HashSet<Guid>();
        private ISet<Language> _languages = new HashSet<Language>();
        private ISet<Interest> _interests = new HashSet<Interest>();
        private ISet<Education> _education = new HashSet<Education>();
        private ISet<Work> _work = new HashSet<Work>();
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName => $"{FirstName} {LastName}";
        public string ProfileImageUrl { get; private set; }
        public string Description { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public bool EmailNotifications { get; private set; }
        public bool IsBanned { get; private set; }
        public State State { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string ContactEmail { get; private set; }
        public string BannerUrl { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Country { get; private set; }
        public string City { get; private set; }

        public bool IsOnline { get; private set; }
        public string DeviceType { get; private set; }
        public DateTime? LastActive { get; private set; }
        public IEnumerable<Language> Languages
        {
            get => _languages;
            set => _languages = new HashSet<Language>(value ?? Enumerable.Empty<Language>());
        }
        public IEnumerable<Interest> Interests
        {
            get => _interests;
            set => _interests = new HashSet<Interest>(value ?? Enumerable.Empty<Interest>());
        }
        public IEnumerable<Education> Education
        {
            get => _education;
            set => _education = new HashSet<Education>(value ?? Enumerable.Empty<Education>());
        }
        public IEnumerable<Work> Work
        {
            get => _work;
            set => _work = new HashSet<Work>(value ?? Enumerable.Empty<Work>());
        }
        public bool IsTwoFactorEnabled { get; private set; }
        public string TwoFactorSecret { get; private set; }
        public IEnumerable<Guid> InterestedInEvents
        {
            get => _interestedInEvents;
            set => _interestedInEvents = new HashSet<Guid>(value ?? Enumerable.Empty<Guid>());
        }
        public IEnumerable<Guid> SignedUpEvents
        {
            get => _signedUpEvents;
            set => _signedUpEvents = new HashSet<Guid>(value ?? Enumerable.Empty<Guid>());
        }

        public Student(Guid id, string email, DateTime createdAt, string firstName, string lastName,
            string profileImageUrl, string description, DateTime? dateOfBirth,
            bool emailNotifications, bool isBanned, State state,
            IEnumerable<Guid> interestedInEvents, IEnumerable<Guid> signedUpEvents,
            string bannerUrl, IEnumerable<Education> education, IEnumerable<Work> work,
            IEnumerable<Language> languages, IEnumerable<Interest> interests,
            bool isTwoFactorEnabled, string twoFactorSecret, string contactEmail,
            string phoneNumber, string country, string city,
            bool isOnline = false, string deviceType = null, DateTime? lastActive = null)
        {
            Id = id;
            Email = email;
            CreatedAt = createdAt;
            FirstName = firstName;
            LastName = lastName;
            ProfileImageUrl = profileImageUrl;
            Description = description;
            DateOfBirth = dateOfBirth;
            EmailNotifications = emailNotifications;
            IsBanned = isBanned;
            State = state;
            InterestedInEvents = interestedInEvents ?? Enumerable.Empty<Guid>();
            SignedUpEvents = signedUpEvents ?? Enumerable.Empty<Guid>();
            BannerUrl = bannerUrl;
            Education = education ?? Enumerable.Empty<Education>();
            Work = work ?? Enumerable.Empty<Work>();
            Languages = languages ?? Enumerable.Empty<Language>();
            Interests = interests ?? Enumerable.Empty<Interest>();
            IsTwoFactorEnabled = isTwoFactorEnabled;
            TwoFactorSecret = twoFactorSecret;
            ContactEmail = contactEmail;
            PhoneNumber = phoneNumber;
            Country = country;
            City = city;
            IsOnline = isOnline;
            DeviceType = deviceType;
            LastActive = lastActive;
        }

        public void SetIncomplete() => SetState(State.Incomplete);
        public void SetValid() => SetState(State.Valid);
        public void SetBanned() => SetState(State.Banned);
        public void SetUnverified() => SetState(State.Unverified);

        private void SetState(State state)
        {
            var previousState = State;
            State = state;
            AddEvent(new StudentStateChanged(this, previousState));
        }

        public void CompleteRegistration(string profileImageUrl, string description,
            DateTime dateOfBirth, DateTime now, bool emailNotifications)
        {
            CheckDescription(description);
            CheckDateOfBirth(dateOfBirth, now);

            if (State != State.Incomplete && State != State.Unverified)
            {
                throw new CannotChangeStudentStateException(Id, State);
            }

            ProfileImageUrl = profileImageUrl;
            Description = description;
            DateOfBirth = dateOfBirth;
            EmailNotifications = emailNotifications;

            State = State.Valid;
            AddEvent(new StudentRegistrationCompleted(this));
        }

        public void Update(string firstName, string lastName, string description,
            bool emailNotifications, string contactEmail, string phoneNumber, string country, string city, DateTime? dateOfBirth)
        {
            CheckFullName(firstName, lastName);
            CheckDescription(description);

            if (State != State.Valid)
            {
                throw new CannotUpdateStudentException(Id);
            }

            FirstName = firstName;
            LastName = lastName;
            Description = description;
            EmailNotifications = emailNotifications;
            ContactEmail = contactEmail;
            PhoneNumber = phoneNumber;
            Country = country;
            City = city;
            DateOfBirth = dateOfBirth;

            AddEvent(new StudentUpdated(this));
        }

        public void UpdateProfileImageUrl(string profileImageUrl)
        {
            ProfileImageUrl = profileImageUrl;
            AddEvent(new StudentUpdated(this));
        }

        public void UpdateBannerUrl(string bannerUrl)
        {
            BannerUrl = bannerUrl;
            AddEvent(new StudentBannerUpdated(this));
        }

        public void UpdateEducation(IEnumerable<Education> education)
        {
            Education = new HashSet<Education>(education ?? Enumerable.Empty<Education>());
            AddEvent(new StudentEducationUpdated(this));
        }

        public void UpdateWork(IEnumerable<Work> work)
        {
            Work = new HashSet<Work>(work ?? Enumerable.Empty<Work>());
            AddEvent(new StudentWorkUpdated(this));
        }

        public void UpdateLanguages(IEnumerable<Language> languages)
        {
            Languages = new HashSet<Language>(languages ?? Enumerable.Empty<Language>());
            AddEvent(new StudentLanguagesUpdated(this));
        }

        public void UpdateInterests(IEnumerable<Interest> interests)
        {
            Interests = new HashSet<Interest>(interests ?? Enumerable.Empty<Interest>());
            AddEvent(new StudentInterestsUpdated(this));
        }

        public void UpdateContactEmail(string contactEmail)
        {
            ContactEmail = contactEmail;
            AddEvent(new StudentUpdated(this));
        }

        public void RemoveProfileImage()
        {
            ProfileImageUrl = string.Empty;
            AddEvent(new StudentProfileImageRemoved(this));
        }

        public void RemoveBannerImage()
        {
            BannerUrl = string.Empty;
            AddEvent(new StudentBannerImageRemoved(this));
        }

        public void EnableTwoFactorAuthentication(string twoFactorSecret)
        {
            if (string.IsNullOrWhiteSpace(twoFactorSecret))
            {
                throw new InvalidTwoFactorSecretException(Id);
            }

            IsTwoFactorEnabled = true;
            TwoFactorSecret = twoFactorSecret;
            AddEvent(new StudentTwoFactorEnabled(this));
        }

        public void DisableTwoFactorAuthentication()
        {
            IsTwoFactorEnabled = false;
            TwoFactorSecret = null;
            AddEvent(new StudentTwoFactorDisabled(this));
        }

        private void CheckFullName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new InvalidStudentFullNameException(Id, $"{firstName} {lastName}");
            }
        }

        private void CheckDescription(string description)
        {
            // if (string.IsNullOrWhiteSpace(description))
            // {
            //     throw new InvalidStudentDescriptionException(Id, description);
            // }
        }

        public void VerifyEmail(string email, DateTime verifiedAt)
        {
            if (Email == email)
            {
                State = State.Valid;
            }
        }

        private void CheckDateOfBirth(DateTime dateOfBirth, DateTime now)
        {
            if (dateOfBirth >= now)
            {
                throw new InvalidStudentDateOfBirthException(Id, dateOfBirth, now);
            }
        }

        public void AddInterestedInEvent(Guid eventId)
        {
            if (eventId == Guid.Empty)
            {
                return;
            }

            if (!_interestedInEvents.Add(eventId))
            {
                throw new StudentAlreadyInterestedInException(Id, eventId);
            }
        }

        public void RemoveInterestedInEvent(Guid eventId)
        {
            if (!_interestedInEvents.Remove(eventId))
            {
                throw new StudentIsNotInterestedException(Id, eventId);
            }
        }

        public void AddSignedUpEvent(Guid eventId)
        {
            if (eventId == Guid.Empty)
            {
                return;
            }

            if (!_signedUpEvents.Add(eventId))
            {
                throw new StudentAlreadySignedUpException(Id, eventId);
            }
        }

        public void RemoveSignedUpEvent(Guid eventId)
        {
            if (!_signedUpEvents.Remove(eventId))
            {
                throw new StudentIsNotSignedUpException(Id, eventId);
            }
        }

        public void Ban() => IsBanned = true;
        public void Unban() => IsBanned = false;

        public void SetEmailNotifications(bool emailNotifications)
        {
            EmailNotifications = emailNotifications;
            AddEvent(new StudentUpdated(this));
        }

        public void SetOnlineStatus(bool isOnline, string deviceType)
        {
            IsOnline = isOnline;
            DeviceType = isOnline ? deviceType : null; 
            LastActive = DateTime.UtcNow;

            AddEvent(new StudentOnlineStatusChanged(this));
        }
    }
}

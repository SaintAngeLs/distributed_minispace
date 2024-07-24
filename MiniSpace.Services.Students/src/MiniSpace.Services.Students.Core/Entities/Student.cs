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
        private ISet<string> _languages = new HashSet<string>();
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
        public FrontendVersion FrontendVersion { get; private set; }
        public PreferredLanguage PreferredLanguage { get; private set; }
        public IEnumerable<string> Languages
        {
            get => _languages;
            set => _languages = new HashSet<string>(value ?? Enumerable.Empty<string>());
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
        public UserSettings Settings { get; private set; }

        public Student(Guid id, string firstName, string lastName, string email, DateTime createdAt)
            : this(id, email, createdAt, firstName, lastName, string.Empty, string.Empty, null,
                false, false, State.Unverified, Enumerable.Empty<Guid>(), Enumerable.Empty<Guid>(), string.Empty,
                Enumerable.Empty<Education>(), Enumerable.Empty<Work>(), Enumerable.Empty<string>(), Enumerable.Empty<Interest>(),
                false, null, string.Empty, string.Empty, FrontendVersion.Auto, PreferredLanguage.English)
        {
            CheckFullName(firstName, lastName);
        }

        public Student(Guid id, string email, DateTime createdAt, string firstName, string lastName,
            string profileImageUrl, string description, DateTime? dateOfBirth,
            bool emailNotifications, bool isBanned, State state,
            IEnumerable<Guid> interestedInEvents, IEnumerable<Guid> signedUpEvents,
            string bannerUrl, IEnumerable<Education> education, IEnumerable<Work> work,
            IEnumerable<string> languages, IEnumerable<Interest> interests,
            bool isTwoFactorEnabled, string twoFactorSecret, string contactEmail,
            string phoneNumber, FrontendVersion frontendVersion, PreferredLanguage preferredLanguage)
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
            Languages = languages ?? Enumerable.Empty<string>();
            Interests = interests ?? Enumerable.Empty<Interest>();
            IsTwoFactorEnabled = isTwoFactorEnabled;
            TwoFactorSecret = twoFactorSecret;
            ContactEmail = contactEmail;
            PhoneNumber = phoneNumber;
            FrontendVersion = frontendVersion;
            PreferredLanguage = preferredLanguage;
            Settings = new UserSettings();
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
            bool emailNotifications, string contactEmail, string phoneNumber, FrontendVersion frontendVersion,
            PreferredLanguage preferredLanguage)
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
            FrontendVersion = frontendVersion;
            PreferredLanguage = preferredLanguage;

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

        public void UpdateLanguages(IEnumerable<string> languages)
        {
            Languages = new HashSet<string>(languages ?? Enumerable.Empty<string>());
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
    }
}

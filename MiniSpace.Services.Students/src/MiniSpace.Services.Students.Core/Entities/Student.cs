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
        private ISet<Guid> _galleryOfImages = new HashSet<Guid>();
        private ISet<string> _languages = new HashSet<string>();
        private ISet<string> _interests = new HashSet<string>();

        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName => $"{FirstName} {LastName}";
        public int NumberOfFriends { get; private set; }
        public Guid ProfileImage { get; private set; }
        public string Description { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public bool EmailNotifications { get; private set; }
        public bool IsBanned { get; private set; }
        public bool IsOrganizer { get; private set; }
        public State State { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Guid? BannerId { get; private set; }
        public IEnumerable<Guid> GalleryOfImages
        {
            get => _galleryOfImages;
            set => _galleryOfImages = new HashSet<Guid>(value ?? Enumerable.Empty<Guid>());
        }
        public string Education { get; private set; }
        public string WorkPosition { get; private set; }
        public string Company { get; private set; }
        public IEnumerable<string> Languages
        {
            get => _languages;
            set => _languages = new HashSet<string>(value ?? Enumerable.Empty<string>());
        }
        public IEnumerable<string> Interests
        {
            get => _interests;
            set => _interests = new HashSet<string>(value ?? Enumerable.Empty<string>());
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

        public Student(Guid id, string firstName, string lastName, string email, DateTime createdAt)
            : this(id, email, createdAt, firstName, lastName, 0, Guid.Empty, string.Empty, null,
                false, false, false, State.Unverified, Enumerable.Empty<Guid>(), Enumerable.Empty<Guid>(), null, 
                Enumerable.Empty<Guid>(), null, null, null, Enumerable.Empty<string>(), Enumerable.Empty<string>(),
                false, null)
        {
            CheckFullName(firstName, lastName);
        }

        public Student(Guid id, string email, DateTime createdAt, string firstName, string lastName,
            int numberOfFriends, Guid profileImage, string description, DateTime? dateOfBirth,
            bool emailNotifications, bool isBanned, bool isOrganizer, State state,
            IEnumerable<Guid> interestedInEvents, IEnumerable<Guid> signedUpEvents,
            Guid? bannerId, IEnumerable<Guid> galleryOfImages, string education,
            string workPosition, string company, IEnumerable<string> languages, IEnumerable<string> interests,
            bool isTwoFactorEnabled, string twoFactorSecret)
        {
            Id = id;
            Email = email;
            CreatedAt = createdAt;
            FirstName = firstName;
            LastName = lastName;
            NumberOfFriends = numberOfFriends;
            ProfileImage = profileImage;
            Description = description;
            DateOfBirth = dateOfBirth;
            EmailNotifications = emailNotifications;
            IsBanned = isBanned;
            IsOrganizer = isOrganizer;
            State = state;
            InterestedInEvents = interestedInEvents ?? Enumerable.Empty<Guid>();
            SignedUpEvents = signedUpEvents ?? Enumerable.Empty<Guid>();
            BannerId = bannerId;
            GalleryOfImages = galleryOfImages ?? Enumerable.Empty<Guid>();
            Education = education;
            WorkPosition = workPosition;
            Company = company;
            Languages = languages ?? Enumerable.Empty<string>();
            Interests = interests ?? Enumerable.Empty<string>();
            IsTwoFactorEnabled = isTwoFactorEnabled;
            TwoFactorSecret = twoFactorSecret;
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

        public void CompleteRegistration(Guid profileImage, string description,
            DateTime dateOfBirth, DateTime now, bool emailNotifications)
        {
            CheckDescription(description);
            CheckDateOfBirth(dateOfBirth, now);

            if (State != State.Incomplete && State != State.Unverified)
            {
                throw new CannotChangeStudentStateException(Id, State);
            }

            ProfileImage = profileImage;
            Description = description;
            DateOfBirth = dateOfBirth;
            EmailNotifications = emailNotifications;

            State = State.Valid;
            AddEvent(new StudentRegistrationCompleted(this));
        }

        public void Update(Guid profileImage, string description, bool emailNotifications)
        {
            CheckDescription(description);

            if (State != State.Valid)
            {
                throw new CannotUpdateStudentException(Id);
            }

            ProfileImage = profileImage;
            Description = description;
            EmailNotifications = emailNotifications;

            AddEvent(new StudentUpdated(this));
        }

        public void UpdateBannerId(Guid? bannerId)
        {
            BannerId = bannerId;
            AddEvent(new StudentBannerUpdated(this));
        }

        public void UpdateGalleryOfImages(IEnumerable<Guid> galleryOfImages)
        {
            GalleryOfImages = new HashSet<Guid>(galleryOfImages ?? Enumerable.Empty<Guid>());
            AddEvent(new StudentGalleryOfImagesUpdated(this));
        }

        public void UpdateEducation(string education)
        {
            Education = education;
            AddEvent(new StudentEducationUpdated(this));
        }

        public void UpdateWorkPosition(string workPosition)
        {
            WorkPosition = workPosition;
            AddEvent(new StudentWorkPositionUpdated(this));
        }

        public void UpdateCompany(string company)
        {
            Company = company;
            AddEvent(new StudentCompanyUpdated(this));
        }

        public void UpdateLanguages(IEnumerable<string> languages)
        {
            Languages = new HashSet<string>(languages ?? Enumerable.Empty<string>());
            AddEvent(new StudentLanguagesUpdated(this));
        }

        public void UpdateInterests(IEnumerable<string> interests)
        {
            Interests = new HashSet<string>(interests ?? Enumerable.Empty<string>());
            AddEvent(new StudentInterestsUpdated(this));
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
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new InvalidStudentDescriptionException(Id, description);
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

        public void RemoveProfileImage(Guid mediaFileId)
        {
            if (ProfileImage != mediaFileId)
            {
                return;
            }

            ProfileImage = Guid.Empty;
        }

        public void Ban() => IsBanned = true;
        public void Unban() => IsBanned = false;
        public void GrantOrganizerRights() => IsOrganizer = true;
        public void RevokeOrganizerRights() => IsOrganizer = false;
    }
}

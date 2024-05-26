using MiniSpace.Services.Students.Core.Events;
using MiniSpace.Services.Students.Core.Exceptions;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class Student : AggregateRoot
    {
        private ISet<Guid> _interestedInEvents = new HashSet<Guid>();
        private ISet<Guid> _signedUpEvents = new HashSet<Guid>();
        
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

        public IEnumerable<Guid> InterestedInEvents
        {
            get => _interestedInEvents;
            set => _interestedInEvents = new HashSet<Guid>(value);
        }
        public IEnumerable<Guid> SignedUpEvents
        {
            get => _signedUpEvents;
            set => _signedUpEvents = new HashSet<Guid>(value);
        }

        public Student(Guid id, string firstName, string lastName, string email, DateTime createdAt)
            : this(id, email, createdAt, firstName, lastName, 0, Guid.Empty, string.Empty, null,
                false, false, false, State.Incomplete, Enumerable.Empty<Guid>(), Enumerable.Empty<Guid>())
        {
            CheckFullName(firstName, lastName);
        }
    
        public Student(Guid id, string email, DateTime createdAt, string firstName, string lastName,
            int numberOfFriends, Guid profileImage, string description, DateTime? dateOfBirth,
            bool emailNotifications, bool isBanned, bool isOrganizer, State state,
            IEnumerable<Guid> interestedInEvents = null, IEnumerable<Guid> signedUpEvents = null)
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
        }
        
        public void SetIncomplete() => SetState(State.Incomplete);
        public void SetValid() => SetState(State.Valid);
        public void SetBanned() => SetState(State.Banned);
        
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
            
            if (State != State.Incomplete)
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

            _interestedInEvents.Add(eventId);
        }
        
        public void RemoveInterestedInEvent(Guid eventId)
            => _interestedInEvents.Remove(eventId);

        public void AddSignedUpEvent(Guid eventId)
        {
            if (eventId == Guid.Empty)
            {
                return;
            }

            _signedUpEvents.Add(eventId);
        }
        
        public void RemoveSignedUpEvent(Guid eventId)
            => _signedUpEvents.Remove(eventId);
        
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

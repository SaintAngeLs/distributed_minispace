namespace MiniSpace.Services.Students.Core.Entities
{
    public class Student : AggregateRoot
    {
        private ISet<Guid> _interestedInEvents = new HashSet<Guid>();
        private ISet<Guid> _signedUpEvents = new HashSet<Guid>();
        
        public string Email { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string FullName => $"{Name} {Surname}";
        public int Friends { get; private set; }
        public string ProfileImage { get; private set; }
        public string Description { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public bool EmailNotifications { get; private set; }
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
        
        public Student(Guid id, string email, DateTime createdAt)
            : this(id, email, createdAt, string.Empty, string.Empty, 0, string.Empty, string.Empty,
                null, false, Enumerable.Empty<Guid>(), Enumerable.Empty<Guid>())
        {}
    
        public Student(Guid id, string email, DateTime createdAt, string name, string surname,
            int friends, string profileImage, string description, DateTime? dateOfBirth, bool emailNotifications,
            IEnumerable<Guid> interestedInEvents = null, IEnumerable<Guid> signedUpEvents = null)
        {
            Id = id;
            Email = email;
            CreatedAt = createdAt;
            Name = name;
            Surname = surname;
            Friends = friends;
            ProfileImage = profileImage;
            Description = description;
            DateOfBirth = dateOfBirth;
            EmailNotifications = emailNotifications;
            InterestedInEvents = interestedInEvents ?? Enumerable.Empty<Guid>();
            SignedUpEvents = signedUpEvents ?? Enumerable.Empty<Guid>();
        }

        public void SetIsBanned(bool isBanned) {}

        public void SetIsOrganizer(bool isOrganizer) {}

        public void AddInterestedInEvent(Guid eventId)
        {
            if (eventId == Guid.Empty)
            {
                return;
            }

            _interestedInEvents.Add(eventId);
        }

        public void AddSignedUpEvent(Guid eventId)
        {
            if (eventId == Guid.Empty)
            {
                return;
            }

            _signedUpEvents.Add(eventId);
        }
    }    
}

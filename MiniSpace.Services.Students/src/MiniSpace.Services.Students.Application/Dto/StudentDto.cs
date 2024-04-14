namespace MiniSpace.Services.Students.Application.Dto
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NumberOfFriends { get; set; }
        public string ProfileImage { get; set; }
        public string Description { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool EmailNotifications { get; set; }
        public bool IsBanned { get; set; }
        public bool CanBeOrganizer { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<Guid> InterestedInEvents { get; set; }
        public IEnumerable<Guid> SignedUpEvents { get; set; }
    }    
}

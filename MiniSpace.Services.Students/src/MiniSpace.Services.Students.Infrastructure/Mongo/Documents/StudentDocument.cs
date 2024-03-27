using Convey.Types;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    public class StudentDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Friends { get; set; }
        public string ProfileImage { get; set; }
        public string Description { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool EmailNotifications { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<Guid> InterestedInEvents { get; set; }
        public IEnumerable<Guid> SignedUpEvents { get; set; }
    }    
}

using Convey.CQRS.Events;

namespace MiniSpace.Services.Friends.Application.Events
{
    public class StudentCreated : IEvent
    {
        public Guid StudentId { get; }
        public string Email { get; }

        public StudentCreated(Guid studentId, string email)
        {
            StudentId = studentId;
            Email = email;
        }
    }
}

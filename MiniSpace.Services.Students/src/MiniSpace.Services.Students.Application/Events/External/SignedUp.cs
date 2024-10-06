using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("identity")]
    public class SignedUp : IEvent
    {
        public Guid UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string Role { get; }
        
        public SignedUp(Guid userId, string firstName, string lastName, string email, string role)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = role;
        }
    }    
}

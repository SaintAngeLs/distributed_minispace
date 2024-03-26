using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("identity")]
    public class SignedUp : IEvent
    {
        public Guid UserId { get; }
        public string Username { get; }
        public string Password { get; }
        public string Email { get; }
        public string Role { get; }
        
        public SignedUp(Guid userId, string username, string password, string email, string role)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Email = email;
            Role = role;
        }
    }    
}

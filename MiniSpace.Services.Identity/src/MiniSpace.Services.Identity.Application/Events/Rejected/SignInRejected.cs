using Paralax.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events.Rejected
{
    
    public class SignInRejected : IRejectedEvent
    {
        public string Email { get; }
        public string Reason { get; }
        public string Code { get; }

        public SignInRejected(string email, string reason, string code)
        {
            Email = email;
            Reason = reason;
            Code = code;
        }
    }
}
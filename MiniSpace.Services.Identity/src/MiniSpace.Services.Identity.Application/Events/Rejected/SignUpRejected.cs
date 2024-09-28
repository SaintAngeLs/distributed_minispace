using Paralax.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events.Rejected
{
    
    public class SignUpRejected : IRejectedEvent
    {
        public string Email { get; }
        public string Reason { get; }
        public string Code { get; }

        public SignUpRejected(string email, string reason, string code)
        {
            Email = email;
            Reason = reason;
            Code = code;
        }
    }
}
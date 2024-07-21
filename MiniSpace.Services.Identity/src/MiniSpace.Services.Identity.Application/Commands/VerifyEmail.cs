using Convey.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    [Contract]
    public class VerifyEmail : ICommand
    {
        public string Token { get; }
        public string Email { get; }
        public string HashedToken { get; }

        public VerifyEmail(string token, string email, string hashedToken)
        {
            Token = token;
            Email = email;
            HashedToken = hashedToken;
        }
    }
}

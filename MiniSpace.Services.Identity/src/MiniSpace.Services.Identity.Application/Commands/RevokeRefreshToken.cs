using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class RevokeRefreshToken : ICommand
    {
        public string RefreshToken { get; }

        public RevokeRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
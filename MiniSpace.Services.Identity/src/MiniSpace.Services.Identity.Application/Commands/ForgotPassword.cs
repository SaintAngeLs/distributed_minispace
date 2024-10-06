using System;
using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class ForgotPassword : ICommand
    {
        public Guid UserId { get; }
        public string Email { get; }

        public ForgotPassword(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }
}

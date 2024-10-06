using System;
using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class ResetPassword : ICommand
    {
        public Guid UserId { get; }
        public string Token { get; }
        public string NewPassword { get; }

        public ResetPassword(Guid userId, string token, string newPassword)
        {
            UserId = userId;
            Token = token;
            NewPassword = newPassword;
        }
    }
}


  
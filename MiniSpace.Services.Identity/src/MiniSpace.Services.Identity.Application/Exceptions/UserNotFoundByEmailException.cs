using System;

namespace MiniSpace.Services.Identity.Application.Exceptions
{
    public class UserNotFoundByEmailException : AppException
    {
        public override string Code { get; } = "user_not_found_by_email";
        public string Email { get; }
        
        public UserNotFoundByEmailException(string email) : base($"User with email: '{email}' was not found.")
        {
            Email = email;
        }
    }
}

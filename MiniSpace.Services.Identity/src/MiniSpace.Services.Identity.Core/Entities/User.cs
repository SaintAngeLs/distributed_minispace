using System;
using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Identity.Core.Exceptions;

namespace MiniSpace.Services.Identity.Core.Entities
{
    public class User : AggregateRoot
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public Role Role { get; private set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; private set; }
        public IEnumerable<string> Permissions { get; private set; }
        public bool IsEmailVerified { get; set; }
        public string EmailVerificationToken { get; set; }
        public DateTime? EmailVerifiedAt { get; set; } 
        public bool IsTwoFactorEnabled { get; set; }
        public string TwoFactorSecret { get; set; }

        public User(Guid id, string name, string email, string password, Role role, DateTime createdAt,
            IEnumerable<string> permissions = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidNameException(name);
            }
            
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidEmailException(email);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidPasswordException();
            }

            if (!role.IsValid())
            {
                throw new InvalidRoleException(role.ToString());
            }

            Id = id;
            Name = name;
            Email = email.ToLowerInvariant();
            Password = password;
            Role = role;
            CreatedAt = createdAt;
            Permissions = permissions ?? Enumerable.Empty<string>();
        }

        internal User(Guid id, string name, string email, string password, Role role, DateTime createdAt,
            bool isEmailVerified, string emailVerificationToken, DateTime? emailVerifiedAt, 
            bool isTwoFactorEnabled, string twoFactorSecret, IEnumerable<string> permissions = null)
            : this(id, name, email, password, role, createdAt, permissions)
        {
            IsEmailVerified = isEmailVerified;
            EmailVerificationToken = emailVerificationToken;
            EmailVerifiedAt = emailVerifiedAt;
            IsTwoFactorEnabled = isTwoFactorEnabled;
            TwoFactorSecret = twoFactorSecret;
        }
                
        public void Ban()
        {
            if (Role == Role.Banned || Role == Role.Admin)
            {
                throw new UserCannotBeBannedException(Id, Role.ToString());
            }

            Role = Role.Banned;
        }
        
        public void Unban()
        {
            if (Role != Role.Banned)
            {
                throw new UserIsNotBannedException(Id, Role.ToString());
            }

            Role = Role.User;
        }

        public void SetEmailVerificationToken(string token)
        {
            if (IsEmailVerified)
            {
                throw new EmailAlreadyVerifiedException();
            }

            EmailVerificationToken = token;
        }

        public void VerifyEmail()
        {
            if (IsEmailVerified)
            {
                throw new EmailAlreadyVerifiedException();
            }

            IsEmailVerified = true;
            EmailVerificationToken = null;
            EmailVerifiedAt = DateTime.UtcNow;
        }

        public void EnableTwoFactorAuthentication(string secret)
        {
            if (IsTwoFactorEnabled)
            {
                throw new TwoFactorAlreadyEnabledException(Id);
            }

            IsTwoFactorEnabled = true;
            TwoFactorSecret = secret;
        }

        public void DisableTwoFactorAuthentication()
        {
            if (!IsTwoFactorEnabled)
            {
                throw new TwoFactorNotEnabledException(Id);
            }

            IsTwoFactorEnabled = false;
            TwoFactorSecret = null;
        }

        public void SetTwoFactorSecret(string secret)
        {
            TwoFactorSecret = secret;
        }
    }

    public static class UserPermissions
    {
        public static string OrganizeEvents { get; private set; } = "organize_events";
    }
    

}

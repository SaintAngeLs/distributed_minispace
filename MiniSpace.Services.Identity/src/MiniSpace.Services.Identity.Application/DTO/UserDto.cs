using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Identity.Core.Entities;

namespace MiniSpace.Services.Identity.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime? EmailVerifiedAt { get; set; } 
        public bool IsTwoFactorEnabled { get; set; }
        public string TwoFactorSecret { get; set; }

        public bool IsOnline { get; set; }
        public string DeviceType { get; set; }
        public DateTime? LastActive { get; set; }

        public string IpAddress { get; set; }

        public UserDto() { }

        public UserDto(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Role = user.Role;
            CreatedAt = user.CreatedAt;
            Permissions = user.Permissions;
            IsEmailVerified = user.IsEmailVerified;
            EmailVerifiedAt = user.EmailVerifiedAt;
            IsTwoFactorEnabled = user.IsTwoFactorEnabled;
            TwoFactorSecret = user.TwoFactorSecret;

            IsOnline = user.IsOnline;
            DeviceType = user.DeviceType;
            LastActive = user.LastActive;

            IpAddress = user.IpAddress;
        }
    }
}

using System;

namespace Astravent.Web.Wasm.DTO
{
    public class UserDto
    {   
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime? EmailVerifiedAt { get; set; } 
        public bool IsTwoFactorEnabled { get; set; }
        public string TwoFactorSecret { get; set; }

        public bool IsOnline { get; set; }
        public string DeviceType { get; set; }
        public DateTime? LastActive { get; set; }
        public string IpAddress { get; set; }
    }
}
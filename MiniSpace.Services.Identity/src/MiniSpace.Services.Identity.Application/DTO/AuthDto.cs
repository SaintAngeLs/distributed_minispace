using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Identity.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class AuthDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
        public long Expires { get; set; }
        public bool IsTwoFactorRequired { get; set; }
        public Guid UserId { get; set; }
        public bool IsOnline { get; set; }
        public string DeviceType { get; set; }
        public string IpAddress { get; set; } 
    }
}

using System;

namespace MiniSpace.Web.DTO
{
    public class JwtDto
    {
        public string AccessToken { get; set; }
        public string Role { get; set; }
        public string RefreshToken { get; set; }
        public long Expires { get; set; }
        public bool IsTwoFactorRequired { get; set; }
        public Guid UserId { get; set; } 
    }
}
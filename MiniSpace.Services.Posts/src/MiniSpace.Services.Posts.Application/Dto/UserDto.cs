using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace MiniSpace.Services.Posts.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Description { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool EmailNotifications { get; set; }
        public bool IsBanned { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ContactEmail { get; set; }
        public string BannerUrl { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Languages { get; set; }
        public IEnumerable<string> Interests { get; set; }
        public IEnumerable<EducationDto> Education { get; set; }
        public IEnumerable<WorkDto> Work { get; set; }
        public bool IsTwoFactorEnabled { get; set; }
        public string TwoFactorSecret { get; set; }
        public IEnumerable<Guid> InterestedInEvents { get; set; }
        public IEnumerable<Guid> SignedUpEvents { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public bool IsOnline { get; set; }           
        public string DeviceType { get; set; }
        public DateTime? LastActive { get; set; } 
    }
}
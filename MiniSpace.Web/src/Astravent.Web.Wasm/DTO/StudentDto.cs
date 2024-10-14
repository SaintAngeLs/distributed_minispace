using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.DTO.Languages;
using Astravent.Web.Wasm.DTO.Interests;

namespace Astravent.Web.Wasm.DTO
{
     public class StudentDto
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
        public List<string> Languages { get; set; } 
        public List<string> Interests { get; set; } 
        public IEnumerable<EducationDto> Education { get; set; }
        public IEnumerable<WorkDto> Work { get; set; }
        
        public WorkDto LastWorkExperience
        {
            get
            {
                // Get the work experiences with OrganizationId set (non-empty Guid)
                var workWithOrganizationId = Work?.Where(w => w.OrganizationId != Guid.Empty)
                                                  .OrderByDescending(w => w.StartDate)
                                                  .FirstOrDefault();
                
                // If found, return the one with OrganizationId, otherwise return the latest work experience
                return workWithOrganizationId ?? Work?.OrderByDescending(w => w.StartDate).FirstOrDefault();
            }
        }
        public bool IsTwoFactorEnabled { get; set; }
        public string TwoFactorSecret { get; set; }
        public IEnumerable<Guid> InterestedInEvents { get; set; }
        public IEnumerable<Guid> SignedUpEvents { get; set; }
        public List<GalleryImageDto> GalleryOfImageUrls { get; set; } 
        public string Country { get; set; }
        public string City { get; set; }
        
        public AvailableSettingsDto UserSettings { get; set; }
        
        public bool IsInvitationPending { get; set; } 
        public bool InvitationSent { get; set; }
        public bool Selected { get; set; }

        public bool IsOnline { get; set; }  
        public string DeviceType { get; set; }  
        public DateTime? LastActive { get; set; }
    }    
}

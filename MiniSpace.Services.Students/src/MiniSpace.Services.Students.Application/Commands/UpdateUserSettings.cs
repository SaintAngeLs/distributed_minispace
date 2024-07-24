using Convey.CQRS.Commands;
using System;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UpdateUserSettings : ICommand
    {
        public Guid StudentId { get; set;}
        public string CreatedAtVisibility { get; set; }
        public string DateOfBirthVisibility { get; set; }
        public string InterestedInEventsVisibility { get; set; }
        public string SignedUpEventsVisibility { get; set; }
        public string EducationVisibility { get; set; }
        public string WorkPositionVisibility { get; set; }
        public string LanguagesVisibility { get; set; }
        public string InterestsVisibility { get; set; }
        public string ContactEmailVisibility { get; set; }
        public string PhoneNumberVisibility { get; set; }
        public string PreferredLanguage { get; set; }
        public string FrontendVersion { get; set; }

        public UpdateUserSettings(Guid studentId, string createdAtVisibility, string dateOfBirthVisibility,
                                  string interestedInEventsVisibility, string signedUpEventsVisibility,
                                  string educationVisibility, string workPositionVisibility, 
                                  string languagesVisibility, string interestsVisibility, 
                                  string contactEmailVisibility, string phoneNumberVisibility, 
                                  string preferredLanguage, string frontendVersion)
        {
            StudentId = studentId;
            CreatedAtVisibility = createdAtVisibility;
            DateOfBirthVisibility = dateOfBirthVisibility;
            InterestedInEventsVisibility = interestedInEventsVisibility;
            SignedUpEventsVisibility = signedUpEventsVisibility;
            EducationVisibility = educationVisibility;
            WorkPositionVisibility = workPositionVisibility;
            LanguagesVisibility = languagesVisibility;
            InterestsVisibility = interestsVisibility;
            ContactEmailVisibility = contactEmailVisibility;
            PhoneNumberVisibility = phoneNumberVisibility;
            PreferredLanguage = preferredLanguage;
            FrontendVersion = frontendVersion;
        }
    }
}

using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class AvailableSettingsDto
    {
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
    }
}

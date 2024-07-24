namespace MiniSpace.Web.DTO
{
    public class AvailableSettingsDto
    {
        public Visibility CreatedAtVisibility { get; set; }
        public Visibility DateOfBirthVisibility { get; set; }
        public Visibility InterestedInEventsVisibility { get; set; }
        public Visibility SignedUpEventsVisibility { get; set; }
        public Visibility EducationVisibility { get; set; }
        public Visibility WorkPositionVisibility { get; set; }
        public Visibility LanguagesVisibility { get; set; }
        public Visibility InterestsVisibility { get; set; }
        public Visibility ContactEmailVisibility { get; set; }
        public Visibility PhoneNumberVisibility { get; set; }
        public PreferredLanguage PreferredLanguage { get; set; }
        public FrontendVersion FrontendVersion { get; set; }
    }
}
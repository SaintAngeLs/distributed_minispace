namespace MiniSpace.Services.Students.Core.Entities
{
    public class UserSettings
    {

        public Guid StudentId { get; set; }
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

        public UserSettings()
        {
            CreatedAtVisibility = Visibility.Everyone;
            DateOfBirthVisibility = Visibility.Everyone;
            InterestedInEventsVisibility = Visibility.Everyone;
            SignedUpEventsVisibility = Visibility.Everyone;
            EducationVisibility = Visibility.Everyone;
            WorkPositionVisibility = Visibility.Everyone;
            LanguagesVisibility = Visibility.Everyone;
            InterestsVisibility = Visibility.Everyone;
            ContactEmailVisibility = Visibility.Everyone;
            PhoneNumberVisibility = Visibility.Everyone;
        }
    }
}

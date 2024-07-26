using System;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class UserAvailableSettings
    {
        public Visibility CreatedAtVisibility { get; private set; }
        public Visibility DateOfBirthVisibility { get; private set; }
        public Visibility InterestedInEventsVisibility { get; private set; }
        public Visibility SignedUpEventsVisibility { get; private set; }
        public Visibility EducationVisibility { get; private set; }
        public Visibility WorkPositionVisibility { get; private set; }
        public Visibility LanguagesVisibility { get; private set; }
        public Visibility InterestsVisibility { get; private set; }
        public Visibility ContactEmailVisibility { get; private set; }
        public Visibility PhoneNumberVisibility { get; private set; }
        public FrontendVersion FrontendVersion { get; private set; }
        public PreferredLanguage PreferredLanguage { get; private set; }

        public UserAvailableSettings()
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
            FrontendVersion = FrontendVersion.Default;
            PreferredLanguage = PreferredLanguage.English;
        }

        public UserAvailableSettings(Visibility createdAtVisibility, Visibility dateOfBirthVisibility, Visibility interestedInEventsVisibility,
                                     Visibility signedUpEventsVisibility, Visibility educationVisibility, Visibility workPositionVisibility,
                                     Visibility languagesVisibility, Visibility interestsVisibility, Visibility contactEmailVisibility,
                                     Visibility phoneNumberVisibility, FrontendVersion frontendVersion, PreferredLanguage preferredLanguage)
        {
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
            FrontendVersion = frontendVersion;
            PreferredLanguage = preferredLanguage;
        }

        public void UpdateSettings(Visibility createdAtVisibility, Visibility dateOfBirthVisibility, Visibility interestedInEventsVisibility,
                                   Visibility signedUpEventsVisibility, Visibility educationVisibility, Visibility workPositionVisibility,
                                   Visibility languagesVisibility, Visibility interestsVisibility, Visibility contactEmailVisibility,
                                   Visibility phoneNumberVisibility, FrontendVersion frontendVersion, PreferredLanguage preferredLanguage)
        {
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
            FrontendVersion = frontendVersion;
            PreferredLanguage = preferredLanguage;
        }
    }
}

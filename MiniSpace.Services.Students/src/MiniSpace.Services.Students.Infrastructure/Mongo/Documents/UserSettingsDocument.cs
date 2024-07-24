using Convey.Types;
using MiniSpace.Services.Students.Core.Entities;
using System;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    public class UserSettingsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
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
    }
}

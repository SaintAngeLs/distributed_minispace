using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UserSettingsRepository : IUserSettingsRepository
    {
        private readonly IMongoRepository<UserSettingsDocument, Guid> _repository;

        public UserSettingsRepository(IMongoRepository<UserSettingsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<UserSettings> GetUserSettingsAsync(Guid studentId)
        {
            var userSettingsDocument = await _repository.GetAsync(x => x.StudentId == studentId);
            if (userSettingsDocument == null)
            {
                return new UserSettings();
            }

            return new UserSettings
            {
                CreatedAtVisibility = userSettingsDocument.CreatedAtVisibility,
                DateOfBirthVisibility = userSettingsDocument.DateOfBirthVisibility,
                InterestedInEventsVisibility = userSettingsDocument.InterestedInEventsVisibility,
                SignedUpEventsVisibility = userSettingsDocument.SignedUpEventsVisibility,
                EducationVisibility = userSettingsDocument.EducationVisibility,
                WorkPositionVisibility = userSettingsDocument.WorkPositionVisibility,
                LanguagesVisibility = userSettingsDocument.LanguagesVisibility,
                InterestsVisibility = userSettingsDocument.InterestsVisibility,
                ContactEmailVisibility = userSettingsDocument.ContactEmailVisibility,
                PhoneNumberVisibility = userSettingsDocument.PhoneNumberVisibility
            };
        }

        public async Task UpdateUserSettingsAsync(Guid studentId, UserSettings userSettings)
        {
            var userSettingsDocument = await _repository.GetAsync(x => x.StudentId == studentId);

            if (userSettingsDocument == null)
            {
                userSettingsDocument = new UserSettingsDocument
                {
                    Id = Guid.NewGuid(),
                    StudentId = studentId,
                    CreatedAtVisibility = userSettings.CreatedAtVisibility,
                    DateOfBirthVisibility = userSettings.DateOfBirthVisibility,
                    InterestedInEventsVisibility = userSettings.InterestedInEventsVisibility,
                    SignedUpEventsVisibility = userSettings.SignedUpEventsVisibility,
                    EducationVisibility = userSettings.EducationVisibility,
                    WorkPositionVisibility = userSettings.WorkPositionVisibility,
                    LanguagesVisibility = userSettings.LanguagesVisibility,
                    InterestsVisibility = userSettings.InterestsVisibility,
                    ContactEmailVisibility = userSettings.ContactEmailVisibility,
                    PhoneNumberVisibility = userSettings.PhoneNumberVisibility
                };

                await _repository.AddAsync(userSettingsDocument);
            }
            else
            {
                userSettingsDocument.CreatedAtVisibility = userSettings.CreatedAtVisibility;
                userSettingsDocument.DateOfBirthVisibility = userSettings.DateOfBirthVisibility;
                userSettingsDocument.InterestedInEventsVisibility = userSettings.InterestedInEventsVisibility;
                userSettingsDocument.SignedUpEventsVisibility = userSettings.SignedUpEventsVisibility;
                userSettingsDocument.EducationVisibility = userSettings.EducationVisibility;
                userSettingsDocument.WorkPositionVisibility = userSettings.WorkPositionVisibility;
                userSettingsDocument.LanguagesVisibility = userSettings.LanguagesVisibility;
                userSettingsDocument.InterestsVisibility = userSettings.InterestsVisibility;
                userSettingsDocument.ContactEmailVisibility = userSettings.ContactEmailVisibility;
                userSettingsDocument.PhoneNumberVisibility = userSettings.PhoneNumberVisibility;

                await _repository.UpdateAsync(userSettingsDocument);
            }
        }
    }
}

using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
    public class GetUserWithVisibilitySettingsHandler : IQueryHandler<GetUserWithVisibilitySettings, StudentWithVisibilitySettingsDto>
    {
        private readonly IMongoRepository<StudentDocument, Guid> _userRepository;
        private readonly IMongoRepository<UserSettingsDocument, Guid> _settingsRepository;

        public GetUserWithVisibilitySettingsHandler(IMongoRepository<StudentDocument, Guid> userRepository, IMongoRepository<UserSettingsDocument, Guid> settingsRepository)
        {
            _userRepository = userRepository;
            _settingsRepository = settingsRepository;
        }

        public async Task<StudentWithVisibilitySettingsDto> HandleAsync(GetUserWithVisibilitySettings query, CancellationToken cancellationToken)
        {
            var studentDocument = await _userRepository.GetAsync(p => p.Id == query.StudentId);
            if (studentDocument == null)
            {
                return null;
            }

            var settingsDocument = await _settingsRepository.GetAsync(s => s.UserId == query.StudentId);
            if (settingsDocument == null)
            {
                return null;
            }

            var visibilitySettings = new AvailableSettingsDto
            {
                CreatedAtVisibility = settingsDocument.AvailableSettings.CreatedAtVisibility.ToString(),
                DateOfBirthVisibility = settingsDocument.AvailableSettings.DateOfBirthVisibility.ToString(),
                InterestedInEventsVisibility = settingsDocument.AvailableSettings.InterestedInEventsVisibility.ToString(),
                SignedUpEventsVisibility = settingsDocument.AvailableSettings.SignedUpEventsVisibility.ToString(),
                EducationVisibility = settingsDocument.AvailableSettings.EducationVisibility.ToString(),
                WorkPositionVisibility = settingsDocument.AvailableSettings.WorkPositionVisibility.ToString(),
                LanguagesVisibility = settingsDocument.AvailableSettings.LanguagesVisibility.ToString(),
                InterestsVisibility = settingsDocument.AvailableSettings.InterestsVisibility.ToString(),
                ContactEmailVisibility = settingsDocument.AvailableSettings.ContactEmailVisibility.ToString(),
                PhoneNumberVisibility = settingsDocument.AvailableSettings.PhoneNumberVisibility.ToString(),
                PreferredLanguage = settingsDocument.AvailableSettings.PreferredLanguage.ToString(),
                FrontendVersion = settingsDocument.AvailableSettings.FrontendVersion.ToString()
            };

            return new StudentWithVisibilitySettingsDto
            {
                User = studentDocument.AsDto(),
                VisibilitySettings = visibilitySettings
            };
        }
    }
}

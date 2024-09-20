using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
    public class GetStudentWithVisibilitySettingsHandler : IQueryHandler<GetStudentWithVisibilitySettings, StudentWithVisibilitySettingsDto>
    {
        private readonly IMongoRepository<StudentDocument, Guid> _studentRepository;
        private readonly IMongoRepository<UserSettingsDocument, Guid> _settingsRepository;

        public GetStudentWithVisibilitySettingsHandler(IMongoRepository<StudentDocument, Guid> studentRepository, IMongoRepository<UserSettingsDocument, Guid> settingsRepository)
        {
            _studentRepository = studentRepository;
            _settingsRepository = settingsRepository;
        }

        public async Task<StudentWithVisibilitySettingsDto> HandleAsync(GetStudentWithVisibilitySettings query, CancellationToken cancellationToken)
        {
            var studentDocument = await _studentRepository.GetAsync(p => p.Id == query.StudentId);
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
                Student = studentDocument.AsDto(),
                VisibilitySettings = visibilitySettings
            };
        }
    }
}

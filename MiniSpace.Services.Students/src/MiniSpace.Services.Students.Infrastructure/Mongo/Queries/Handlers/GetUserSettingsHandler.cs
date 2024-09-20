using Convey.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
    public class GetUserSettingsHandler : IQueryHandler<GetUserSettings, UserSettingsDto>
    {
        private readonly IUserSettingsRepository _userSettingsRepository;

        public GetUserSettingsHandler(IUserSettingsRepository userSettingsRepository)
        {
            _userSettingsRepository = userSettingsRepository;
        }

        public async Task<UserSettingsDto> HandleAsync(GetUserSettings query, CancellationToken cancellationToken)
        {
            var userSettings = await _userSettingsRepository.GetUserSettingsAsync(query.StudentId);
            if (userSettings == null)
            {
                return null;
            }

            return new UserSettingsDto
            {
                UserId = userSettings.UserId,
                CreatedAtVisibility = userSettings.AvailableSettings.CreatedAtVisibility.ToString(),
                DateOfBirthVisibility = userSettings.AvailableSettings.DateOfBirthVisibility.ToString(),
                InterestedInEventsVisibility = userSettings.AvailableSettings.InterestedInEventsVisibility.ToString(),
                SignedUpEventsVisibility = userSettings.AvailableSettings.SignedUpEventsVisibility.ToString(),
                EducationVisibility = userSettings.AvailableSettings.EducationVisibility.ToString(),
                WorkPositionVisibility = userSettings.AvailableSettings.WorkPositionVisibility.ToString(),
                LanguagesVisibility = userSettings.AvailableSettings.LanguagesVisibility.ToString(),
                InterestsVisibility = userSettings.AvailableSettings.InterestsVisibility.ToString(),
                ContactEmailVisibility = userSettings.AvailableSettings.ContactEmailVisibility.ToString(),
                PhoneNumberVisibility = userSettings.AvailableSettings.PhoneNumberVisibility.ToString(),
                PreferredLanguage = userSettings.AvailableSettings.PreferredLanguage.ToString(),
                FrontendVersion = userSettings.AvailableSettings.FrontendVersion.ToString()
            };
        }
    }
}

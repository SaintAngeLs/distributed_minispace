using Convey.CQRS.Commands;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class UpdateUserSettingsHandler : ICommandHandler<UpdateUserSettings>
    {
        private readonly IUserSettingsRepository _userSettingsRepository;
        private readonly IStudentRepository _studentRepository;

        public UpdateUserSettingsHandler(IUserSettingsRepository userSettingsRepository, IStudentRepository studentRepository)
        {
            _userSettingsRepository = userSettingsRepository;
            _studentRepository = studentRepository;
        }

        public async Task HandleAsync(UpdateUserSettings command, CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetAsync(command.StudentId);
            if (student is null)
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            var userSettings = await _userSettingsRepository.GetUserSettingsAsync(command.StudentId);
            if (userSettings is null)
            {
                throw new UserSettingsNotFoundException(command.StudentId);
            }

            var availableSettings = new UserAvailableSettings(
                Enum.Parse<Visibility>(command.AvailableSettings.CreatedAtVisibility),
                Enum.Parse<Visibility>(command.AvailableSettings.DateOfBirthVisibility),
                Enum.Parse<Visibility>(command.AvailableSettings.InterestedInEventsVisibility),
                Enum.Parse<Visibility>(command.AvailableSettings.SignedUpEventsVisibility),
                Enum.Parse<Visibility>(command.AvailableSettings.EducationVisibility),
                Enum.Parse<Visibility>(command.AvailableSettings.WorkPositionVisibility),
                Enum.Parse<Visibility>(command.AvailableSettings.LanguagesVisibility),
                Enum.Parse<Visibility>(command.AvailableSettings.InterestsVisibility),
                Enum.Parse<Visibility>(command.AvailableSettings.ContactEmailVisibility),
                Enum.Parse<Visibility>(command.AvailableSettings.PhoneNumberVisibility),
                Enum.Parse<FrontendVersion>(command.AvailableSettings.FrontendVersion),
                Enum.Parse<PreferredLanguage>(command.AvailableSettings.PreferredLanguage)
            );

            userSettings.UpdateSettings(availableSettings);

            await _userSettingsRepository.UpdateUserSettingsAsync(userSettings);
        }
    }
}

using Paralax.CQRS.Commands;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class UpdateUserSettingsHandler : ICommandHandler<UpdateUserSettings>
    {
        private readonly IUserSettingsRepository _userSettingsRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAppContext _appContext;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public UpdateUserSettingsHandler(
            IUserSettingsRepository userSettingsRepository,
            IStudentRepository studentRepository,
            IAppContext appContext,
            IEventMapper eventMapper,
            IMessageBroker messageBroker)
        {
            _userSettingsRepository = userSettingsRepository;
            _studentRepository = studentRepository;
            _appContext = appContext;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdateUserSettings command, CancellationToken cancellationToken = default)
        {
            var commandJson = JsonSerializer.Serialize(command, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine("Received UpdateUserSettings command:");
            Console.WriteLine(commandJson);

            var student = await _studentRepository.GetAsync(command.StudentId);
            if (student == null)
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            var userSettings = await _userSettingsRepository.GetUserSettingsAsync(command.StudentId);
            if (userSettings == null)
            {
                throw new UserSettingsNotFoundException(command.StudentId);
            }

            var availableSettings = new UserAvailableSettings(
                Enum.Parse<Visibility>(command.CreatedAtVisibility, true),
                Enum.Parse<Visibility>(command.DateOfBirthVisibility, true),
                Enum.Parse<Visibility>(command.InterestedInEventsVisibility, true),
                Enum.Parse<Visibility>(command.SignedUpEventsVisibility, true),
                Enum.Parse<Visibility>(command.EducationVisibility, true),
                Enum.Parse<Visibility>(command.WorkPositionVisibility, true),
                Enum.Parse<Visibility>(command.LanguagesVisibility, true),
                Enum.Parse<Visibility>(command.InterestsVisibility, true),
                Enum.Parse<Visibility>(command.ContactEmailVisibility, true),
                Enum.Parse<Visibility>(command.PhoneNumberVisibility, true),
                Enum.Parse<Visibility>(command.ProfileImageVisibility, true),
                Enum.Parse<Visibility>(command.BannerImageVisibility, true),
                Enum.Parse<Visibility>(command.GalleryVisibility, true),
                Enum.Parse<FrontendVersion>(command.FrontendVersion, true),
                Enum.Parse<PreferredLanguage>(command.PreferredLanguage, true),
                Enum.Parse<Visibility>(command.ConnectionVisibility, true),
                Enum.Parse<Visibility>(command.FollowersVisibility, true),
                Enum.Parse<Visibility>(command.FollowingVisibility, true),
                Enum.Parse<Visibility>(command.MyPostsVisibility, true),
                Enum.Parse<Visibility>(command.ConnectionsPostsVisibility, true),
                Enum.Parse<Visibility>(command.MyRepostsVisibility, true),
                Enum.Parse<Visibility>(command.RepostsOfMyConnectionsVisibility, true),
                Enum.Parse<Visibility>(command.OrganizationIAmCreatorVisibility, true),
                Enum.Parse<Visibility>(command.OrganizationIFollowVisibility, true)
            );

            userSettings.UpdateSettings(availableSettings);
            await _userSettingsRepository.UpdateUserSettingsAsync(userSettings);

            var events = _eventMapper.MapAll(userSettings.Events);
            await _messageBroker.PublishAsync(events);
        }
    }
}

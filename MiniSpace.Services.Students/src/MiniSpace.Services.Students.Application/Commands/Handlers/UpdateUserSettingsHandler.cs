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
        private readonly IUserRepository _userRepository;
        private readonly IAppContext _appContext;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public UpdateUserSettingsHandler(
            IUserSettingsRepository userSettingsRepository,
            IUserRepository userRepository,
            IAppContext appContext,
            IEventMapper eventMapper,
            IMessageBroker messageBroker)
        {
            _userSettingsRepository = userSettingsRepository;
            _userRepository = userRepository;
            _appContext = appContext;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdateUserSettings command, CancellationToken cancellationToken = default)
        {
            var commandJson = JsonSerializer.Serialize(command, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine("Received UpdateUserSettings command:");
            Console.WriteLine(commandJson);

            var student = await _userRepository.GetAsync(command.UserId);
            if (student == null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            var userSettings = await _userSettingsRepository.GetUserSettingsAsync(command.UserId);
            if (userSettings == null)
            {
                throw new UserSettingsNotFoundException(command.UserId);
            }

            var availableSettings = new UserAvailableSettings(
                Enum.Parse<Visibility>(EnsureValue(command.CreatedAtVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.DateOfBirthVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.InterestedInEventsVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.SignedUpEventsVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.EducationVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.WorkPositionVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.LanguagesVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.InterestsVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.ContactEmailVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.PhoneNumberVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.ProfileImageVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.BannerImageVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.GalleryVisibility, "Everyone"), true),
                // Here we use non-visibility values directly since the UI should supply these:
                Enum.Parse<FrontendVersion>(EnsureValue(command.FrontendVersion, "Default"), true),
                Enum.Parse<PreferredLanguage>(EnsureValue(command.PreferredLanguage, "English"), true),
                Enum.Parse<Visibility>(EnsureValue(command.ConnectionVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.FollowersVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.FollowingVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.FriendListVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.FollowersListVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.FollowingListVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.MyPostsVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.ConnectionsPostsVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.MyRepostsVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.RepostsOfMyConnectionsVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.OrganizationIAmCreatorVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.OrganizationIFollowVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.IsOnlineVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.DeviceTypeVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.LastActiveVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.CountryVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.CityVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.MessageVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.ProfileVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.PostCommentVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.PostLikeVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.FriendRequestVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.TaggedPostVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.StoryVisibility, "Everyone"), true),
                Enum.Parse<Visibility>(EnsureValue(command.GroupMembershipVisibility, "Everyone"), true)
            );


            userSettings.UpdateSettings(availableSettings);
            await _userSettingsRepository.UpdateUserSettingsAsync(userSettings);

            var events = _eventMapper.MapAll(userSettings.Events);
            await _messageBroker.PublishAsync(events);
        }
        
        private string EnsureValue(string value, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

    }
}

using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class SignedUpHandler : IEventHandler<SignedUp>
    {
        private const string RequiredRole = "User";
        private readonly IStudentRepository _studentRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<SignedUpHandler> _logger;
        private readonly IUserNotificationPreferencesRepository _notificationPreferencesRepository;
        private readonly IUserSettingsRepository _userSettingsRepository;

        public SignedUpHandler(IStudentRepository studentRepository, IDateTimeProvider dateTimeProvider,
            ILogger<SignedUpHandler> logger, IUserNotificationPreferencesRepository notificationPreferencesRepository,
            IUserSettingsRepository userSettingsRepository)
        {
            _studentRepository = studentRepository;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
            _notificationPreferencesRepository = notificationPreferencesRepository;
            _userSettingsRepository = userSettingsRepository;
        }

        public async Task HandleAsync(SignedUp @event, CancellationToken cancellationToken = default)
        {
            if (@event.Role != RequiredRole)
            {
                throw new InvalidRoleException(@event.UserId, @event.Role, RequiredRole);
            }

            var student = await _studentRepository.GetAsync(@event.UserId);
            if (student is not null)
            {
                throw new StudentAlreadyCreatedException(student.Id);
            }

            var newStudent = new Student(
                @event.UserId,
                @event.Email,
                _dateTimeProvider.Now,
                @event.FirstName,
                @event.LastName,
                string.Empty, // ProfileImageUrl
                string.Empty, // Description
                null, // DateOfBirth
                false, // EmailNotifications
                false, // IsBanned
                State.Unverified, // State
                new List<Guid>(), // InterestedInEvents
                new List<Guid>(), // SignedUpEvents
                string.Empty, // BannerUrl
                new List<Education>(), // Education
                new List<Work>(), // Work
                new List<Language>(), // Languages
                new List<Interest>(), // Interests
                false, // IsTwoFactorEnabled
                string.Empty, // TwoFactorSecret
                string.Empty, // ContactEmail
                string.Empty, // PhoneNumber,
                string.Empty, // Country
                string.Empty // City 
            );

            await _studentRepository.AddAsync(newStudent);

            var defaultPreferences = new NotificationPreferences();
            await _notificationPreferencesRepository.UpdateNotificationPreferencesAsync(newStudent.Id, defaultPreferences);

            var defaultAvailableSettings = new UserAvailableSettings();
            var userSettings = new UserSettings(newStudent.Id, defaultAvailableSettings);
            await _userSettingsRepository.AddUserSettingsAsync(userSettings);

            _logger.LogInformation($"New student created with ID: {@event.UserId}, default notification preferences set, and default user settings initialized.");
        }
    }
}

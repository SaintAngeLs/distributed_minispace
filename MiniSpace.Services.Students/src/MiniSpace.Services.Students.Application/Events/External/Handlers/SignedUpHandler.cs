using Paralax.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers;

public class SignedUpHandler : IEventHandler<SignedUp>
{
    private const string RequiredRole = "User";
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<SignedUpHandler> _logger;
    private readonly IUserNotificationPreferencesRepository _notificationPreferencesRepository;
    private readonly IUserSettingsRepository _userSettingsRepository;

    public SignedUpHandler(
        IUserRepository userRepository,
        IDateTimeProvider dateTimeProvider,
        ILogger<SignedUpHandler> logger,
        IUserNotificationPreferencesRepository notificationPreferencesRepository,
        IUserSettingsRepository userSettingsRepository)
    {
        _userRepository = userRepository;
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

        var existingUser = await _userRepository.GetAsync(@event.UserId);
        if (existingUser is not null)
        {
            throw new StudentAlreadyCreatedException(existingUser.Id);
        }

        // Generate a username based on first and last name (or use @event.UserName if available)
        var userName = $"{@event.FirstName}.{@event.LastName}".ToLowerInvariant();

        var newUser = new User(
            @event.UserId,
            @event.Email,
            userName,                      // New: Username parameter
            _dateTimeProvider.Now,
            @event.FirstName,
            @event.LastName,
            string.Empty,                  // ProfileImageUrl
            string.Empty,                  // Description
            null,                          // DateOfBirth
            false,                         // EmailNotifications
            false,                         // IsBanned
            State.Unverified,              // State
            new List<Guid>(),              // InterestedInEvents
            new List<Guid>(),              // SignedUpEvents
            string.Empty,                  // BannerUrl
            new List<Education>(),         // Education
            new List<Work>(),              // Work
            new List<Language>(),          // Languages
            new List<Interest>(),          // Interests
            false,                         // IsTwoFactorEnabled
            string.Empty,                  // TwoFactorSecret
            string.Empty,                  // ContactEmail
            string.Empty,                  // PhoneNumber
            string.Empty,                  // Country
            string.Empty,                  // City
            false,                         // IsOnline (default to offline)
            string.Empty,                  // DeviceType (default)
            null                           // LastActive (default to null)
        );

        await _userRepository.AddAsync(newUser);

        var defaultPreferences = new NotificationPreferences();
        await _notificationPreferencesRepository.UpdateNotificationPreferencesAsync(newUser.Id, defaultPreferences);

        var defaultAvailableSettings = new UserAvailableSettings();
        var userSettings = new UserSettings(newUser.Id, defaultAvailableSettings);
        await _userSettingsRepository.AddUserSettingsAsync(userSettings);

        _logger.LogInformation($"New user created with ID: {@event.UserId}, default notification preferences set, and default user settings initialized.");
    }
}

using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class SignedUpHandler : IEventHandler<SignedUp>
    {
        private const string RequiredRole = "user";
        private readonly IStudentRepository _studentRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<SignedUpHandler> _logger;
        private readonly IUserNotificationPreferencesRepository _notificationPreferencesRepository;

        public SignedUpHandler(IStudentRepository studentRepository, IDateTimeProvider dateTimeProvider,
            ILogger<SignedUpHandler> logger, IUserNotificationPreferencesRepository notificationPreferencesRepository)
        {
            _studentRepository = studentRepository;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
            _notificationPreferencesRepository = notificationPreferencesRepository;
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

            var newStudent = new Student(@event.UserId, @event.FirstName, @event.LastName,
                @event.Email, _dateTimeProvider.Now);
            await _studentRepository.AddAsync(newStudent);

            var defaultPreferences = new NotificationPreferences();
            await _notificationPreferencesRepository.UpdateNotificationPreferencesAsync(newStudent.Id, defaultPreferences);

            _logger.LogInformation($"New student created with ID: {@event.UserId} and default notification preferences set.");
        }
    }
}

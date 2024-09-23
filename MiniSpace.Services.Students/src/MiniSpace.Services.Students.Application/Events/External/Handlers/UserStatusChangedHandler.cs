using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class UserStatusChangedHandler : IEventHandler<UserStatusChanged>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<UserStatusChangedHandler> _logger;

        public UserStatusChangedHandler(IStudentRepository studentRepository, ILogger<UserStatusChangedHandler> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        public async Task HandleAsync(UserStatusChanged @event, CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetAsync(@event.UserId);
            if (student is null)
            {
                _logger.LogWarning($"Student with ID '{@event.UserId}' not found.");
                return;
            }

            student.SetOnlineStatus(@event.IsOnline, @event.DeviceType);
            student.UpdateLastActive();
            await _studentRepository.UpdateAsync(student);

            _logger.LogInformation($"Student '{@event.UserId}' status changed. Online: {@event.IsOnline}, Device: {@event.DeviceType}, IP: {@event.IpAddress}");
        }
    }
}

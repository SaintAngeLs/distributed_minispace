using Paralax.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class UserStatusChangedHandler : IEventHandler<UserStatusChanged>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserStatusChangedHandler> _logger;

        public UserStatusChangedHandler(IUserRepository userRepository, ILogger<UserStatusChangedHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task HandleAsync(UserStatusChanged @event, CancellationToken cancellationToken = default)
        {
            var student = await _userRepository.GetAsync(@event.UserId);
            if (student is null)
            {
                _logger.LogWarning($"Student with ID '{@event.UserId}' not found.");
                return;
            }

            student.SetOnlineStatus(@event.IsOnline, @event.DeviceType);
            student.UpdateLastActive();
            await _userRepository.UpdateAsync(student);

            _logger.LogInformation($"Student '{@event.UserId}' status changed. Online: {@event.IsOnline}, Device: {@event.DeviceType}, IP: {@event.IpAddress}");
        }
    }
}

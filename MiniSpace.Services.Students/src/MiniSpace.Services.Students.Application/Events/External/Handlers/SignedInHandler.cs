using Paralax.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class SignedInHandler : IEventHandler<SignedIn>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<SignedInHandler> _logger;

        public SignedInHandler(IUserRepository userRepository, ILogger<SignedInHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task HandleAsync(SignedIn @event, CancellationToken cancellationToken = default)
        {
            var student = await _userRepository.GetAsync(@event.UserId);
            if (student is null)
            {
                _logger.LogWarning($"User with ID '{@event.UserId}' not found.");
                return;
            }

            student.SetOnlineStatus(true, @event.DeviceType);
            student.UpdateLastActive();
            await _userRepository.UpdateAsync(student);

            _logger.LogInformation($"User '{@event.UserId}' is now online. Device: {@event.DeviceType}, IP: {@event.IpAddress}");
        }
    }
}

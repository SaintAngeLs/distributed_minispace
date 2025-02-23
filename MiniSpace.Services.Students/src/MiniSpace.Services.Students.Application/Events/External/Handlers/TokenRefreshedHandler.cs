using Paralax.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class TokenRefreshedHandler : IEventHandler<TokenRefreshed>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TokenRefreshedHandler> _logger;

        public TokenRefreshedHandler(IUserRepository userRepository, ILogger<TokenRefreshedHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task HandleAsync(TokenRefreshed @event, CancellationToken cancellationToken = default)
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

            _logger.LogInformation($"User '{@event.UserId}' refreshed token. Device: {@event.DeviceType}, IP: {@event.IpAddress}");
        }
    }
}

using Paralax.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class SignedOutHandler : IEventHandler<SignedOut>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<SignedOutHandler> _logger;

        public SignedOutHandler(IUserRepository userRepository, ILogger<SignedOutHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task HandleAsync(SignedOut @event, CancellationToken cancellationToken = default)
        {
            var student = await _userRepository.GetAsync(@event.UserId);
            if (student is null)
            {
                _logger.LogWarning($"Student with ID '{@event.UserId}' not found.");
                return;
            }

            student.SetOnlineStatus(false, null);
            await _userRepository.UpdateAsync(student);

            _logger.LogInformation($"Student '{@event.UserId}' is now offline. Device: {@event.DeviceType}");
        }
    }
}

using Paralax.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class TwoFactorAuthenticationDisabledHandler : IEventHandler<TwoFactorAuthenticationDisabled>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TwoFactorAuthenticationDisabledHandler> _logger;

        public TwoFactorAuthenticationDisabledHandler(IUserRepository userRepository, ILogger<TwoFactorAuthenticationDisabledHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task HandleAsync(TwoFactorAuthenticationDisabled @event, CancellationToken cancellationToken = default)
        {
            var student = await _userRepository.GetAsync(@event.UserId);
            if (student == null)
            {
                _logger.LogWarning($"Student with ID: {@event.UserId} not found.");
                return;
            }

            student.DisableTwoFactorAuthentication();
            await _userRepository.UpdateAsync(student);

            _logger.LogInformation($"Two-factor authentication disabled for student with ID: {@event.UserId}.");
        }
    }
}

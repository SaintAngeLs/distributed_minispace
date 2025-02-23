using Paralax.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class EmailVerifiedHandler : IEventHandler<EmailVerified>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<EmailVerifiedHandler> _logger;

        public EmailVerifiedHandler(IUserRepository userRepository, ILogger<EmailVerifiedHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task HandleAsync(EmailVerified @event, CancellationToken cancellationToken = default)
        {
            var student = await _userRepository.GetAsync(@event.UserId);
            if (student == null)
            {
                _logger.LogWarning($"User with ID: {@event.UserId} not found.");
                return;
            }

            student.VerifyEmail(@event.Email, @event.VerifiedAt);
            await _userRepository.UpdateAsync(student);

            _logger.LogInformation($"Email verified for student with ID: {@event.UserId}.");
        }
    }
}

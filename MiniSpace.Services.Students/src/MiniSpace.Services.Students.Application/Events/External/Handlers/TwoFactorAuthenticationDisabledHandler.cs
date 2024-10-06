using Paralax.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class TwoFactorAuthenticationDisabledHandler : IEventHandler<TwoFactorAuthenticationDisabled>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<TwoFactorAuthenticationDisabledHandler> _logger;

        public TwoFactorAuthenticationDisabledHandler(IStudentRepository studentRepository, ILogger<TwoFactorAuthenticationDisabledHandler> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        public async Task HandleAsync(TwoFactorAuthenticationDisabled @event, CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetAsync(@event.UserId);
            if (student == null)
            {
                _logger.LogWarning($"Student with ID: {@event.UserId} not found.");
                return;
            }

            student.DisableTwoFactorAuthentication();
            await _studentRepository.UpdateAsync(student);

            _logger.LogInformation($"Two-factor authentication disabled for student with ID: {@event.UserId}.");
        }
    }
}

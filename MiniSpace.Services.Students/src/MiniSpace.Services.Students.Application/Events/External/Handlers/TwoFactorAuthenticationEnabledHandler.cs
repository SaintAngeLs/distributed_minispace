using Paralax.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class TwoFactorAuthenticationEnabledHandler : IEventHandler<TwoFactorAuthenticationEnabled>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<TwoFactorAuthenticationEnabledHandler> _logger;

        public TwoFactorAuthenticationEnabledHandler(IStudentRepository studentRepository, ILogger<TwoFactorAuthenticationEnabledHandler> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        public async Task HandleAsync(TwoFactorAuthenticationEnabled @event, CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetAsync(@event.UserId);
            if (student == null)
            {
                _logger.LogWarning($"Student with ID: {@event.UserId} not found.");
                return;
            }

            student.EnableTwoFactorAuthentication(@event.Secret);
            await _studentRepository.UpdateAsync(student);

            _logger.LogInformation($"Two-factor authentication enabled for student with ID: {@event.UserId}.");
        }
    }
}

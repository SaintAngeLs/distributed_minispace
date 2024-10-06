using Paralax.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class EmailVerifiedHandler : IEventHandler<EmailVerified>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<EmailVerifiedHandler> _logger;

        public EmailVerifiedHandler(IStudentRepository studentRepository, ILogger<EmailVerifiedHandler> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        public async Task HandleAsync(EmailVerified @event, CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetAsync(@event.UserId);
            if (student == null)
            {
                _logger.LogWarning($"Student with ID: {@event.UserId} not found.");
                return;
            }

            student.VerifyEmail(@event.Email, @event.VerifiedAt);
            await _studentRepository.UpdateAsync(student);

            _logger.LogInformation($"Email verified for student with ID: {@event.UserId}.");
        }
    }
}

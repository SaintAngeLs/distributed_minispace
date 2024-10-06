using Paralax.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class SignedOutHandler : IEventHandler<SignedOut>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<SignedOutHandler> _logger;

        public SignedOutHandler(IStudentRepository studentRepository, ILogger<SignedOutHandler> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        public async Task HandleAsync(SignedOut @event, CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetAsync(@event.UserId);
            if (student is null)
            {
                _logger.LogWarning($"Student with ID '{@event.UserId}' not found.");
                return;
            }

            student.SetOnlineStatus(false, null);
            await _studentRepository.UpdateAsync(student);

            _logger.LogInformation($"Student '{@event.UserId}' is now offline. Device: {@event.DeviceType}");
        }
    }
}

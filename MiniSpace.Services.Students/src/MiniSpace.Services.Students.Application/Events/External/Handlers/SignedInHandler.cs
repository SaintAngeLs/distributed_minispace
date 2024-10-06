using Paralax.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class SignedInHandler : IEventHandler<SignedIn>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<SignedInHandler> _logger;

        public SignedInHandler(IStudentRepository studentRepository, ILogger<SignedInHandler> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        public async Task HandleAsync(SignedIn @event, CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetAsync(@event.UserId);
            if (student is null)
            {
                _logger.LogWarning($"Student with ID '{@event.UserId}' not found.");
                return;
            }

            student.SetOnlineStatus(true, @event.DeviceType);
            student.UpdateLastActive();
            await _studentRepository.UpdateAsync(student);

            _logger.LogInformation($"Student '{@event.UserId}' is now online. Device: {@event.DeviceType}, IP: {@event.IpAddress}");
        }
    }
}

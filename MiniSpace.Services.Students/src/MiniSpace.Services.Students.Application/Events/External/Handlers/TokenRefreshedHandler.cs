using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class TokenRefreshedHandler : IEventHandler<TokenRefreshed>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<TokenRefreshedHandler> _logger;

        public TokenRefreshedHandler(IStudentRepository studentRepository, ILogger<TokenRefreshedHandler> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        public async Task HandleAsync(TokenRefreshed @event, CancellationToken cancellationToken = default)
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

            _logger.LogInformation($"Student '{@event.UserId}' refreshed token. Device: {@event.DeviceType}, IP: {@event.IpAddress}");
        }
    }
}

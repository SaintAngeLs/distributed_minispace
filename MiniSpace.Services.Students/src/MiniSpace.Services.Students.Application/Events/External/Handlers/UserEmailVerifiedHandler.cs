using Paralax.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class UserEmailVerifiedHandler : IEventHandler<EmailVerified>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserEmailVerifiedHandler> _logger;

        public UserEmailVerifiedHandler(IUserRepository userRepository, ILogger<UserEmailVerifiedHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task HandleAsync(EmailVerified @event, CancellationToken cancellationToken = default)
        {
            var student = await _userRepository.GetAsync(@event.UserId);
            if (student == null)
            {
                _logger.LogError($"User with ID {@event.UserId} not found.");
                throw new UserNotFoundException(@event.UserId);
            }

            student.SetValid();
            await _userRepository.UpdateAsync(student);

            _logger.LogInformation($"User with ID {@event.UserId} email verified and state set to valid.");
        }
    }
}

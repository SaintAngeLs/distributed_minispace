using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class StudentEmailVerifiedHandler : IEventHandler<EmailVerified>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<StudentEmailVerifiedHandler> _logger;

        public StudentEmailVerifiedHandler(IStudentRepository studentRepository, ILogger<StudentEmailVerifiedHandler> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        public async Task HandleAsync(EmailVerified @event, CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetAsync(@event.UserId);
            if (student == null)
            {
                _logger.LogError($"Student with ID {@event.UserId} not found.");
                throw new StudentNotFoundException(@event.UserId);
            }

            student.SetValid();
            await _studentRepository.UpdateAsync(student);

            _logger.LogInformation($"Student with ID {@event.UserId} email verified and state set to valid.");
        }
    }
}

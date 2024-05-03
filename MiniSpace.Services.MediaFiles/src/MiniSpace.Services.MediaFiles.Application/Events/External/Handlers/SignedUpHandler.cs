using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;

namespace MiniSpace.Services.MediaFiles.Application.Events.External.Handlers
{
    public class SignedUpHandler : IEventHandler<SignedUp>
    {
        private const string RequiredRole = "user";
        private readonly IStudentRepository _studentRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<SignedUpHandler> _logger;
        
        public SignedUpHandler(IStudentRepository studentRepository, IDateTimeProvider dateTimeProvider,
            ILogger<SignedUpHandler> logger)
        {
            _studentRepository = studentRepository;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task HandleAsync(SignedUp @event, CancellationToken cancellationToken = default)
        {
            if (@event.Role != RequiredRole)
            {
                throw new InvalidRoleException(@event.UserId, @event.Role, RequiredRole);
            }

            var student = await _studentRepository.GetAsync(@event.UserId);
            if (student is not null)
            {
                throw new StudentAlreadyCreatedException(student.Id);
            }

            var newStudent = new Student(@event.UserId, @event.FirstName, @event.LastName,
                @event.Email, _dateTimeProvider.Now);
            await _studentRepository.AddAsync(newStudent);
        }
    }    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class SignedUpHandler : IEventHandler<SignedUp>
    {
        private const string RequiredRole = "student";
        private readonly IStudentRepository _studentRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<SignedUpHandler> _logger;
        
        public SignedUpHandler(IStudentRepository studentRepository, IDateTimeProvider dateTimeProvider,
            IMessageBroker messageBroker, ILogger<SignedUpHandler> logger)
        {
            _studentRepository = studentRepository;
            _dateTimeProvider = dateTimeProvider;
            _messageBroker = messageBroker;
            _logger = logger;
        }

        public async Task HandleAsync(SignedUp @event, CancellationToken cancellationToken = default)
        {
            if (@event.Role != RequiredRole)
            {
                throw new InvalidRoleException(@event.UserId, @event.Role, RequiredRole);
            }

            var student = await _studentRepository.GetAsync(@event.UserId);
            if (student != null)
            {
                throw new StudentAlreadyCreatedException(student.Id);
            }

            var newStudent = new Student(@event.UserId, @event.Username, @event.Password,
                @event.Email, _dateTimeProvider.Now);
            await _studentRepository.AddAsync(newStudent);
            
            await _messageBroker.PublishAsync(new StudentCreated(newStudent.Id));
        }
    }    
}

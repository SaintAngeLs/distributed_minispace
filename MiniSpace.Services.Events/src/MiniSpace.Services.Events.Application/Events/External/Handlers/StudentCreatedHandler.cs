using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Events.External.Handlers
{
    public class StudentCreatedHandler : IEventHandler<StudentCreated>
    {
        private readonly IStudentRepository _studentRepository;

        public StudentCreatedHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        
        public async Task HandleAsync(StudentCreated @event, CancellationToken cancellationToken)
        {
            if (await _studentRepository.ExistsAsync(@event.StudentId))
            {
                throw new StudentAlreadyAddedException(@event.StudentId);
            }
            
            await _studentRepository.AddAsync(new Student(@event.StudentId));
        }
    }
}
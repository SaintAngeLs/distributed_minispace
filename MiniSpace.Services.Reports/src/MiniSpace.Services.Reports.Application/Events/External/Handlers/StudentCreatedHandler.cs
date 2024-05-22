using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Reports.Application.Exceptions;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Core.Repositories;

namespace MiniSpace.Services.Reports.Application.Events.External.Handlers
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
            
            await _studentRepository.AddAsync(new Student(@event.StudentId,0));
        }
    }
}
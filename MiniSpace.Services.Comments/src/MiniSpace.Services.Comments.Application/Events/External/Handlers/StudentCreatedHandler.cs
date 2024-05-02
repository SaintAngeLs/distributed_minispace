using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;

namespace MiniSpace.Services.Comments.Application.Events.External.Handlers
{
    public class StudentCreatedHandler : IEventHandler<StudentCreated>
    {
        private readonly IStudentRepository _studentRepository;

        public StudentCreatedHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        
        public async Task HandleAsync(StudentCreated @event, CancellationToken cancellationToken = default)
        {
            if (await _studentRepository.ExistsAsync(@event.StudentId))
            {
                throw new StudentAlreadyExistsException(@event.StudentId);
            }

            await _studentRepository.AddAsync(new Student(@event.StudentId, @event.FullName));
        }
    }    
}

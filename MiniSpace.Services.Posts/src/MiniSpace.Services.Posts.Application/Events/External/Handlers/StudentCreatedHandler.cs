using Convey.CQRS.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;

namespace MiniSpace.Services.Posts.Application.Events.External.Handlers
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
                throw new StudentAlreadyAddedException(@event.StudentId);
            }

            await _studentRepository.AddAsync(new Student(@event.StudentId, @event.FullName));
        }
    }    
}

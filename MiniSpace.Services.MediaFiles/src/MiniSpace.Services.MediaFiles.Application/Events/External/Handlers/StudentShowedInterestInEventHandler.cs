using Convey.CQRS.Events;
using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Core.Repositories;

namespace MiniSpace.Services.MediaFiles.Application.Events.External.Handlers
{
    public class StudentShowedInterestInEventHandler : IEventHandler<StudentShowedInterestInEvent>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public StudentShowedInterestInEventHandler(IStudentRepository studentRepository,
            IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _studentRepository = studentRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(StudentShowedInterestInEvent @event, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetAsync(@event.StudentId);
            if (student is null)
            {
                throw new StudentNotFoundException(@event.StudentId);
            }
            
            student.AddInterestedInEvent(@event.EventId);
            await _studentRepository.UpdateAsync(student);
            
            var events = _eventMapper.MapAll(student.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }    
}

using Convey.CQRS.Events;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class OrganizerRightsGrantedHandler : IEventHandler<OrganizerRightsGranted>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public OrganizerRightsGrantedHandler(IStudentRepository studentRepository,
            IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _studentRepository = studentRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(OrganizerRightsGranted @event, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetAsync(@event.UserId);
            if (student is null)
            {
                throw new StudentNotFoundException(@event.UserId);
            }
            
            student.GrantOrganizerRights();
            await _studentRepository.UpdateAsync(student);
            
            var events = _eventMapper.MapAll(student.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }    
}

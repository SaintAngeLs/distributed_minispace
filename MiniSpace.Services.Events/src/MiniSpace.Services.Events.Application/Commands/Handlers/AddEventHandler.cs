using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Services;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class AddEventHandler: ICommandHandler<AddEvent>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        
        public AddEventHandler(IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }
        
        public async Task HandleAsync(AddEvent command)
        {
            
        }
    }
}
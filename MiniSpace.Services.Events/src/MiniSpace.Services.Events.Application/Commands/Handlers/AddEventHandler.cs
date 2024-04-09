using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class AddEventHandler: ICommandHandler<AddEvent>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly string _expectedFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
        
        public AddEventHandler(IEventRepository eventRepository, IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _eventRepository = eventRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }
        
        public async Task HandleAsync(AddEvent command)
        {
            if (!Enum.TryParse<Category>(command.Category, true, out var category))
            {
                throw new InvalidEventCategoryException(command.Category);
            }
            if (!DateTime.TryParseExact(command.StartDate, _expectedFormat, CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, out DateTime startDate))
            {
                throw new InvalidEventDateTimeException("event_start_date",command.StartDate);
            }
            if (!DateTime.TryParseExact(command.EndDate, _expectedFormat, CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, out DateTime endDate))
            {
                throw new InvalidEventDateTimeException("event_end_date", command.EndDate);
            }
            
            var address = new Address(command.BuildingName, command.Street, command.BuildingNumber, 
                command.ApartmentNumber, command.City, command.ZipCode);
            var activity = new Event(command.EventId, command.Name, command.Description,
                startDate, endDate, address, command.Capacity, command.Fee, category);
        }
    }
}
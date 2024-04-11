using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
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
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEventValidator _eventValidator;
        
        public AddEventHandler(IEventRepository eventRepository, IMessageBroker messageBroker, IEventMapper eventMapper,
            IDateTimeProvider dateTimeProvider, IEventValidator eventValidator)
        {
            _eventRepository = eventRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _dateTimeProvider = dateTimeProvider;
            _eventValidator = eventValidator;
        }
        
        public async Task HandleAsync(AddEvent command)
        {
            var category = _eventValidator.ParseCategory(command.Category);
            var startDate = _eventValidator.ParseDate(command.StartDate, "event_start_date");
            var endDate = _eventValidator.ParseDate(command.EndDate, "event_end_date");
            var now = _dateTimeProvider.Now;
            _eventValidator.ValidateDates(now, startDate, "now", "event_start_date");
            _eventValidator.ValidateDates(startDate, endDate, "event_start_date", "event_end_date");
            var publishDate = now;
            var status = State.Published;
            if (command.PublishDate != null)
            {
                publishDate = _eventValidator.ParseDate(command.PublishDate, "event_publish_date");
                _eventValidator.ValidateDates(now, publishDate, "now", "event_publish_date");
                status = State.ToBePublished;
            }
            
            var address = new Address(command.BuildingName, command.Street, command.BuildingNumber, 
                command.ApartmentNumber, command.City, command.ZipCode);
            var @event = Event.Create(command.EventId, command.Name, command.Description, startDate, endDate, 
                address, command.Capacity, command.Fee, category, status, publishDate, command.OrganizerId);
            
            await _eventRepository.AddAsync(@event);
            // TODO: update mapper
            var events = _eventMapper.MapAll(@event.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}
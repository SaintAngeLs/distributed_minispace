using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class UpdateEventHandler : ICommandHandler<UpdateEvent>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventValidator _eventValidator;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        private readonly IDateTimeProvider _dateTimeProvider;
        
        public UpdateEventHandler(IEventRepository eventRepository, IEventValidator eventValidator, 
            IAppContext appContext, IMessageBroker messageBroker, IDateTimeProvider dateTimeProvider)
        {
            _eventRepository = eventRepository;
            _eventValidator = eventValidator;
            _appContext = appContext;
            _messageBroker = messageBroker;
            _dateTimeProvider = dateTimeProvider;
        }
        
        public async Task HandleAsync(UpdateEvent command, CancellationToken cancellationToken)
        {
            var @event = await _eventRepository.GetAsync(command.EventId);
            if (@event is null)
            {
                throw new EventNotFoundException(command.EventId);
            }
            
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && !@event.IsOrganizer(identity.Id) && !identity.IsAdmin)
            {
                throw new UnauthorizedEventAccessException(@event.Id, identity.Id);
            }
            
            var name = command.Name == string.Empty ? @event.Name : command.Name;
            var description = command.Description == string.Empty ? @event.Description : command.Description;
            var category = command.Category == string.Empty ? @event.Category : _eventValidator.ParseCategory(command.Category);
            var startDate = command.StartDate == string.Empty ? @event.StartDate 
                : _eventValidator.ParseDate(command.StartDate, "event_start_date");
            var endDate = command.EndDate == string.Empty ? @event.EndDate 
                : _eventValidator.ParseDate(command.EndDate, "event_end_date");
            var now = _dateTimeProvider.Now;
            _eventValidator.ValidateDates(now, startDate, "now", "event_start_date");
            _eventValidator.ValidateDates(startDate, endDate, "event_start_date", "event_end_date");
            
            var address = @event.Location.Update(command.BuildingName, command.Street, command.BuildingNumber, 
                command.ApartmentNumber, command.City, command.ZipCode);
            var capacity = command.Capacity == 0 ? @event.Capacity : command.Capacity;
            _eventValidator.ValidateUpdatedCapacity(capacity, @event.Capacity);
            var fee = command.Fee == 0 ? @event.Fee : command.Fee;
            _eventValidator.ValidateUpdatedFee(fee, @event.Fee);
            
            var publishDate = @event.PublishDate;
            var state = @event.State;
            if (command.PublishDate != string.Empty)
            {
                publishDate = _eventValidator.ParseDate(command.PublishDate, "event_publish_date");
                _eventValidator.ValidateDates(now, publishDate, "now", "event_publish_date");
                _eventValidator.ValidateDates(publishDate, startDate, "event_publish_date", "event_start_date");
                state = State.ToBePublished;
            }
            
            @event.Update(name, description, startDate, endDate, address, capacity, fee, category, state, publishDate, now);
            await _eventRepository.UpdateAsync(@event);
            await _messageBroker.PublishAsync(new EventUpdated(@event.Id, _dateTimeProvider.Now, identity.Id));
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Services.Clients;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class AddEventHandler: ICommandHandler<AddEvent>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IOrganizationsServiceClient _organizationsServiceClient;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEventValidator _eventValidator;
        private readonly IAppContext _appContext;
        
        public AddEventHandler(IEventRepository eventRepository, IMessageBroker messageBroker, 
            IOrganizationsServiceClient organizationsServiceClient, IDateTimeProvider dateTimeProvider, 
            IEventValidator eventValidator, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _messageBroker = messageBroker;
            _organizationsServiceClient = organizationsServiceClient;
            _dateTimeProvider = dateTimeProvider;
            _eventValidator = eventValidator;
            _appContext = appContext;
        }
        
        public async Task HandleAsync(AddEvent command, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if (!identity.IsOrganizer)
                throw new AuthorizedUserIsNotAnOrganizerException(identity.Id);
            if(identity.Id != command.OrganizerId)
                throw new OrganizerCannotAddEventForAnotherOrganizerException(identity.Id, command.OrganizerId);
            
            _eventValidator.ValidateName(command.Name);
            _eventValidator.ValidateDescription(command.Description);
            var startDate = _eventValidator.ParseDate(command.StartDate, "event_start_date");
            var endDate = _eventValidator.ParseDate(command.EndDate, "event_end_date");
            var now = _dateTimeProvider.Now;
            _eventValidator.ValidateDates(now, startDate, "now", "event_start_date");
            _eventValidator.ValidateDates(startDate, endDate, "event_start_date", "event_end_date");
            var address = new Address(command.BuildingName, command.Street, command.BuildingNumber, 
                command.ApartmentNumber, command.City, command.ZipCode);
            _eventValidator.ValidateCapacity(command.Capacity);
            _eventValidator.ValidateFee(command.Fee);
            var category = _eventValidator.ParseCategory(command.Category);
            
            var publishDate = now;
            var state = State.Published;
            if (command.PublishDate != string.Empty)
            {
                publishDate = _eventValidator.ParseDate(command.PublishDate, "event_publish_date");
                _eventValidator.ValidateDates(now, publishDate, "now", "event_publish_date");
                _eventValidator.ValidateDates(publishDate, startDate, "event_publish_date", "event_start_date");
                state = State.ToBePublished;
            }
            
            var organization = await _organizationsServiceClient.GetAsync(command.OrganizationId);
            if (organization == null)
            {
                throw new OrganizationNotFoundException(command.OrganizationId);
            }
            
            if (!organization.Organizers.Contains(command.OrganizerId))
            {
                throw new OrganizerDoesNotBelongToOrganizationException(command.OrganizerId, command.OrganizationId);
            }
            
            var organizer = new Organizer(command.OrganizerId, identity.Name, identity.Email, command.OrganizerId, string.Empty);
            var @event = Event.Create(command.EventId, command.Name, command.Description, startDate, endDate, 
                address, command.Capacity, command.Fee, category, state, publishDate, organizer, now);
            
            await _eventRepository.AddAsync(@event);
            await _messageBroker.PublishAsync(new EventCreated(@event.Id, @event.Organizer.Id));
        }
    }
}
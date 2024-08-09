using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Services.Clients;
using MiniSpace.Services.Events.Application.Wrappers;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventValidator _eventValidator;
        private readonly IOrganizationsServiceClient _organizationsServiceClient;
        private readonly IAppContext _appContext;

        public EventService(IEventRepository eventRepository, IEventValidator eventValidator, 
            IOrganizationsServiceClient organizationsServiceClient, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _eventValidator = eventValidator;
            _organizationsServiceClient = organizationsServiceClient;
            _appContext = appContext;
        }

        public async Task<PagedResponse<IEnumerable<EventDto>>> BrowseEventsAsync(SearchEvents command)
        {
            var dateFrom = DateTime.MinValue;
            var dateTo = DateTime.MinValue;
            Category? category = null;
            State? state = null;
            EventEngagementType? friendsEngagementType = null;
            IEnumerable<Guid> organizations = new List<Guid>();
            if(command.DateFrom != string.Empty)
            {
                dateFrom =_eventValidator.ParseDate(command.DateFrom, "DateFrom");
            }
            if(command.DateTo != string.Empty)
            {
                dateTo = _eventValidator.ParseDate(command.DateTo, "DateTo");
            }
            if(command.Category != string.Empty)
            {
                category = _eventValidator.ParseCategory(command.Category);
            }
            if(command.State != string.Empty)
            {
                state = _eventValidator.ParseState(command.State);
                state = _eventValidator.RestrictState(state);
            }
            if(command.FriendsEngagementType != string.Empty)
            {
                friendsEngagementType = _eventValidator.ParseEngagementType(command.FriendsEngagementType);
            }
            if (command.OrganizationId != Guid.Empty)
            {
                organizations = await _organizationsServiceClient
                    .GetAllChildrenOrganizations(command.OrganizationId) ?? new List<Guid>();
            }
            (int pageNumber, int pageSize) = _eventValidator.PageFilter(command.Pageable.Page, command.Pageable.Size);
            
            var result = await _eventRepository.BrowseEventsAsync(
                pageNumber, pageSize, command.Name, command.Organizer, dateFrom, dateTo, category, state, organizations,
                command.Friends, friendsEngagementType, command.Pageable.Sort.SortBy, command.Pageable.Sort.Direction);
            
            var identity = _appContext.Identity;
            var pagedEvents = new PagedResponse<IEnumerable<EventDto>>(result.events.Select(e => new EventDto(e, identity.Id)), 
                result.pageNumber, result.pageSize, result.totalPages, result.totalElements);

            return pagedEvents;
        }
        
        public async Task<PagedResponse<IEnumerable<EventDto>>> BrowseOrganizerEventsAsync(SearchOrganizerEvents command)
        {
            var identity = _appContext.Identity;
            if(identity.IsAuthenticated && identity.Id != command.OrganizerId && !identity.IsAdmin)
            {
                throw new UnauthorizedOrganizerEventsAccessException(command.OrganizerId, identity.Id);
            }
            var dateFrom = DateTime.MinValue;
            var dateTo = DateTime.MinValue;
            State? state = null;
            if(command.DateFrom != string.Empty)
            {
                dateFrom =_eventValidator.ParseDate(command.DateFrom, "DateFrom");
            }
            if(command.DateTo != string.Empty)
            {
                dateTo = _eventValidator.ParseDate(command.DateTo, "DateTo");
            }
            if(command.State != string.Empty)
            {
                state = _eventValidator.ParseState(command.State);
            }
            (int pageNumber, int pageSize) = _eventValidator.PageFilter(command.Pageable.Page, command.Pageable.Size);
            
            var result = await _eventRepository.BrowseOrganizerEventsAsync(
                pageNumber, pageSize, command.Name, command.OrganizerId, dateFrom, dateTo, 
                command.Pageable.Sort.SortBy, command.Pageable.Sort.Direction, state);
            
            var pagedEvents = new PagedResponse<IEnumerable<EventDto>>(result.events.Select(e => new EventDto(e, _appContext.Identity.Id)), 
                result.pageNumber, result.pageSize, result.totalPages, result.totalElements);

            return pagedEvents;
        }
    }
}
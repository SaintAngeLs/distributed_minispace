using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Wrappers;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Infrastructure.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventValidator _eventValidator;
        private readonly IAppContext _appContext;

        public EventService(IEventRepository eventRepository, IEventValidator eventValidator, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _eventValidator = eventValidator;
            _appContext = appContext;
        }

        public async Task<PagedResponse<IEnumerable<EventDto>>> BrowseEventsAsync(SearchEvents command)
        {
            var dateFrom = DateTime.MinValue;
            var dateTo = DateTime.MinValue;
            if(command.DateFrom != string.Empty)
            {
                dateFrom =_eventValidator.ParseDate(command.DateFrom, "DateFrom");
            }
            if(command.DateTo != string.Empty)
            {
                dateTo = _eventValidator.ParseDate(command.DateTo, "DateTo");
            }
            (int pageNumber, int pageSize) = _eventValidator.PageFilter(command.Pageable.Page, command.Pageable.Size);
            
            var result = await _eventRepository.BrowseAsync(
                pageNumber, pageSize, command.Name, command.Organizer, dateFrom, dateTo, 
                command.Pageable.Sort.SortBy, command.Pageable.Sort.Direction, State.Published);
            
            var identity = _appContext.Identity;
            var pagedEvents = new PagedResponse<IEnumerable<EventDto>>(result.Item1.Select(e => new EventDto(e, identity.Id)), 
                result.Item2, result.Item3, result.Item4, result.Item5);

            return pagedEvents;
        }
    }
}
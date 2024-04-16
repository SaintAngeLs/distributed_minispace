using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Wrappers;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class SearchEventsHandler: ICommandHandler<SearchEvents>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventValidator _eventValidator;
        private readonly IMessageBroker _messageBroker;

        public SearchEventsHandler(IEventRepository eventRepository, IEventValidator eventValidator, IMessageBroker messageBroker)
        {
            _eventRepository = eventRepository;
            _eventValidator = eventValidator;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(SearchEvents command)
        {
            var dateTo = DateTime.MinValue;
            var dateFrom = DateTime.MinValue;
            if(command.DateTo != null)
            {
                _eventValidator.ParseDate(command.DateTo, "DateTo");
            }
            if(command.DateFrom != null)
            {
                _eventValidator.ParseDate(command.DateFrom, "DateFrom");
            }
            (int pageNumber, int pageSize) = _eventValidator.PageFilter(command.Pageable.Page, command.Pageable.Size);
            
            var result = await _eventRepository.BrowseAsync(
                pageNumber, pageSize, command.Name, command.Organizer, dateFrom, dateTo, 
                command.Pageable.Sort.SortBy, command.Pageable.Sort.Direction);
            
            var pagedEvents = new PagedResponse<IEnumerable<EventDto>>(result.Item1.Select(e => new EventDto(e)), 
                result.Item2, result.Item3, result.Item4, result.Item5);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Wrappers;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Services.Events
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventValidator _eventValidator;
        private readonly IMessageBroker _messageBroker;

        public EventService(IEventRepository eventRepository, IEventValidator eventValidator, IMessageBroker messageBroker)
        {
            _eventRepository = eventRepository;
            _eventValidator = eventValidator;
            _messageBroker = messageBroker;
        }

        public async Task<PagedResponse<IEnumerable<EventDto>>> SignInAsync(SearchEvents command)
        {
            var dateTo = DateTime.MinValue;
            var dateFrom = DateTime.MinValue;
            if(command.DateTo != string.Empty)
            {
                _eventValidator.ParseDate(command.DateTo, "DateTo");
            }
            if(command.DateFrom != string.Empty)
            {
                _eventValidator.ParseDate(command.DateFrom, "DateFrom");
            }
            (int pageNumber, int pageSize) = _eventValidator.PageFilter(command.Pageable.Page, command.Pageable.Size);
            
            var result = await _eventRepository.BrowseAsync(
                pageNumber, pageSize, command.Name, command.Organizer, dateFrom, dateTo, 
                command.Pageable.Sort.SortBy, command.Pageable.Sort.Direction, State.Published);
            
            var pagedEvents = new PagedResponse<IEnumerable<EventDto>>(result.Item1.Select(e => new EventDto(e)), 
                result.Item2, result.Item3, result.Item4, result.Item5);

            return pagedEvents;
        }
    }
}
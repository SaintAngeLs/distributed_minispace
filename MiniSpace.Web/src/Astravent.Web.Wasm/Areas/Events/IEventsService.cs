using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.DTO.Wrappers;
using Astravent.Web.Wasm.Areas.Events.CommandsDto;
using Astravent.Web.Wasm.HttpClients;
using Astravent.Web.Wasm.Areas.PagedResult;

namespace Astravent.Web.Wasm.Areas.Events
{
    public interface IEventsService
    {
        Task<EventDto> GetEventAsync(Guid eventId);
        Task<HttpResponse<object>> CreateEventAsync(CreateEventCommand command);
        Task<HttpResponse<object>> UpdateEventAsync(UpdateEventCommand command);
        Task DeleteEventAsync(Guid eventId);
        Task SignUpToEventAsync(SignUpToEventCommand command);
        Task CancelSignUpToEventAsync(CancelSignUpToEventCommand command);
        Task ShowInterestInEventAsync(ShowInterestInEventCommand command);
        Task CancelInterestInEventAsync(CancelInterestInEventCommand command);
        Task RateEventAsync(RateEventCommand command);
        Task<EventRatingDto> GetEventRatingAsync(Guid eventId);
        
        // Task<HttpResponse<PagedResult<IEnumerable<EventDto>>>> SearchEventsAsync(SearchEvents command);
        Task<PagedResult<EventDto>> SearchEventsAsync(SearchEvents command);
        Task<HttpResponse<PagedResult<IEnumerable<EventDto>>>> SearchOrganizerEventsAsync(SearchOrganizerEvents command);
        Task<EventParticipantsDto> GetEventParticipantsAsync(Guid eventId);
        Task AddEventParticipantAsync(Guid eventId, Guid studentId, string studentName);
        Task RemoveEventParticipantAsync(Guid eventId, Guid participantId);
        Task<PagedResult<EventDto>> GetPaginatedEventsAsync(int page, int pageSize);
        Task<PagedResult<EventDto>> GetMyEventsAsync(Guid organizerId, int page, int pageSize); 
        Task<PagedResult<EventDto>> GetUserEventsAsync(Guid userId, int page, int pageSize, string engagementType);
        Task<PagedResult<EventDto>> GetUserEventsFeedAsync(Guid userId, int pageNumber, int pageSize, string sortBy, string direction);
        Task ViewEventAsync(ViewEventCommand command);
    }
}

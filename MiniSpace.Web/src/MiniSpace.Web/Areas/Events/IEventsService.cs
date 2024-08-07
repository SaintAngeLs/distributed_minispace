using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.Areas.Events.CommandsDto;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Events
{
    public interface IEventsService
    {
        Task<EventDto> GetEventAsync(Guid eventId);
        Task<HttpResponse<object>> CreateEventAsync(CreateEventCommand command);
        Task<HttpResponse<object>> UpdateEventAsync(UpdateEventCommand command);
        Task DeleteEventAsync(Guid eventId);
        Task SignUpToEventAsync(Guid eventId, Guid studentId);
        Task CancelSignUpToEventAsync(Guid eventId, Guid studentId);
        Task ShowInterestInEventAsync(Guid eventId, Guid studentId);
        Task CancelInterestInEventAsync(Guid eventId, Guid studentId);
        Task RateEventAsync(Guid eventId, int rating, Guid studentId);
        Task<EventRatingDto> GetEventRatingAsync(Guid eventId);
        Task<HttpResponse<PagedResponseDto<IEnumerable<EventDto>>>> SearchEventsAsync(SearchEvents command);
        Task<HttpResponse<PagedResponseDto<IEnumerable<EventDto>>>> SearchOrganizerEventsAsync(SearchOrganizerEvents command);
        Task<EventParticipantsDto> GetEventParticipantsAsync(Guid eventId);
        Task AddEventParticipantAsync(Guid eventId, Guid studentId, string studentName);
        Task RemoveEventParticipantAsync(Guid eventId, Guid participantId);
    }
}

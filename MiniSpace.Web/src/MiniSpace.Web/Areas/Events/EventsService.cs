using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
using MiniSpace.Web.Areas.Events.CommandsDto;
using MiniSpace.Web.HttpClients;
using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.Areas.Events
{
    public class EventsService : IEventsService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;
        
        public EventsService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }
        
        public Task<EventDto> GetEventAsync(Guid eventId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<EventDto>($"events/{eventId}");
        }

        public Task<HttpResponse<object>> CreateEventAsync(CreateEventCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<CreateEventCommand, object>("events", command);
        }

        public Task<HttpResponse<object>> UpdateEventAsync(UpdateEventCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync<UpdateEventCommand, object>($"events/{command.EventId}", command);
        }

        public Task DeleteEventAsync(Guid eventId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"events/{eventId}");
        }

        public Task SignUpToEventAsync(Guid eventId, Guid studentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync($"events/{eventId}/sign-up", new {eventId, studentId});
        }

        public Task CancelSignUpToEventAsync(Guid eventId, Guid studentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"events/{eventId}/sign-up", new {studentId});
        }

        public Task ShowInterestInEventAsync(Guid eventId, Guid studentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync($"events/{eventId}/show-interest", new {eventId, studentId});
        }

        public Task CancelInterestInEventAsync(Guid eventId, Guid studentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"events/{eventId}/show-interest", new {studentId});
        }

        public Task RateEventAsync(Guid eventId, int rating, Guid studentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync($"events/{eventId}/rate", new {eventId, rating, studentId});
        }

        public Task<EventRatingDto> GetEventRatingAsync(Guid eventId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<EventRatingDto>($"events/{eventId}/rating");
        }

        public Task<HttpResponse<PagedResponseDto<IEnumerable<EventDto>>>> SearchEventsAsync(SearchEvents command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<SearchEvents, PagedResponseDto<IEnumerable<EventDto>>>("events/search", command);
        }

        public Task<HttpResponse<PagedResponseDto<IEnumerable<EventDto>>>> SearchOrganizerEventsAsync(SearchOrganizerEvents command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<SearchOrganizerEvents, PagedResponseDto<IEnumerable<EventDto>>>("events/search/organizer", command);
        }

        // Implementations for participant-related methods
        public Task<EventParticipantsDto> GetEventParticipantsAsync(Guid eventId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<EventParticipantsDto>($"events/{eventId}/participants");
        }

        public Task AddEventParticipantAsync(Guid eventId, Guid studentId, string studentName)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync($"events/{eventId}/participants", new {eventId, studentId, studentName});
        }

        public Task RemoveEventParticipantAsync(Guid eventId, Guid participantId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"events/{eventId}/participants?participantId={participantId}");
        }
    }
}

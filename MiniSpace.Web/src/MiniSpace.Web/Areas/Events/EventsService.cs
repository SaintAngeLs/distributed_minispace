using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
using MiniSpace.Web.Data.Events;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Events
{
    public class EventsService: IEventsService
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
            return _httpClient.GetAsync<EventDto>($"events/{eventId}");
        }

        public Task<PagedResponseDto<IEnumerable<EventDto>>> GetStudentEventsAsync(Guid studentId,
            string engagementType, int page, int numberOfResults)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<PagedResponseDto<IEnumerable<EventDto>>>(
                $"events/student/{studentId}?engagementType={engagementType}&page={page}&numberOfResults={numberOfResults}");
        }

        public Task<HttpResponse<object>> AddEventAsync(Guid eventId, string name, Guid organizerId, Guid organizationId,
            Guid rootOrganizationId, string startDate, string endDate, string buildingName, string street,
            string buildingNumber, string apartmentNumber, string city, string zipCode, string description,
            int capacity, decimal fee, string category, string publishDate)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object,object>("events", new {eventId, name, organizerId, organizationId,
                rootOrganizationId, startDate, endDate, buildingName, street, buildingNumber, apartmentNumber, city,
                zipCode, description, capacity, fee, category, publishDate});
        }

        public Task<HttpResponse<object>> UpdateEventAsync(Guid eventId, string name, Guid organizerId, string startDate, string endDate,
            string buildingName, string street, string buildingNumber, string apartmentNumber, string city, string zipCode,
            string description, int capacity, decimal fee, string category, string publishDate)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync<object, object>($"events/{eventId}", new {eventId, name, organizerId,
                startDate, endDate, buildingName, street, buildingNumber, apartmentNumber, city, zipCode, description,
                capacity, fee, category, publishDate});
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
            return _httpClient.DeleteAsync($"events/{eventId}/sign-up?studentId={studentId}");
        }
        
        public Task ShowInterestInEventAsync(Guid eventId, Guid studentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync($"events/{eventId}/show-interest", new {eventId, studentId});
        }

        public Task CancelInterestInEventAsync(Guid eventId, Guid studentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"events/{eventId}/show-interest?studentId={studentId}");
        }
        
        public Task RateEventAsync(Guid eventId, int rating, Guid studentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync($"events/{eventId}/rate", new {eventId, rating, studentId});
        }
        
        public Task<HttpResponse<PagedResponseDto<IEnumerable<EventDto>>>> SearchEventsAsync(string name, string organizer,
            Guid organizationId, Guid rootOrganizationId, string category, string state, IEnumerable<Guid> friends,
            string friendsEngagementType, string dateFrom, string dateTo, PageableDto pageable)
        {
            return _httpClient.PostAsync<SearchEvents, PagedResponseDto<IEnumerable<EventDto>>>("events/search", 
                new (name, organizer, organizationId, rootOrganizationId, category, state, friends,
                    friendsEngagementType, dateFrom, dateTo, pageable));
        }

        public Task<HttpResponse<PagedResponseDto<IEnumerable<EventDto>>>> SearchOrganizerEventsAsync(Guid organizerId,
            string name, string state, string dateFrom, string dateTo, PageableDto pageable)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<SearchOrganizerEvents, PagedResponseDto<IEnumerable<EventDto>>>("events/search/organizer", 
                new (name, organizerId, dateFrom, dateTo, state, pageable));
        }

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
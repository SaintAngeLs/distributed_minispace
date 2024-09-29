using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.Areas.Identity;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.Areas.Events.CommandsDto;
using Astravent.Web.Wasm.HttpClients;
using Astravent.Web.Wasm.DTO.Wrappers;
using Astravent.Web.Wasm.Areas.PagedResult;
using System.Web;
using System.Linq;
using Blazorise;

namespace Astravent.Web.Wasm.Areas.Events
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
        
        public async Task<EventDto> GetEventAsync(Guid eventId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<EventDto>($"events/{eventId}");
        }

        public async Task<HttpResponse<object>> CreateEventAsync(CreateEventCommand command)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<CreateEventCommand, object>("events", command);
        }

        public async Task<HttpResponse<object>> UpdateEventAsync(UpdateEventCommand command)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PutAsync<UpdateEventCommand, object>($"events/{command.EventId}", command);
        }

        public async Task DeleteEventAsync(Guid eventId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.DeleteAsync($"events/{eventId}");
        }

        public async Task SignUpToEventAsync(SignUpToEventCommand command)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PostAsync($"events/{command.EventId}/sign-up", command);
        }

        public async Task CancelSignUpToEventAsync(CancelSignUpToEventCommand command)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.DeleteAsync($"events/{command.EventId}/sign-up", command);
        }

        public async Task ShowInterestInEventAsync(ShowInterestInEventCommand command)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PostAsync($"events/{command.EventId}/show-interest", command);
        }

        public async Task CancelInterestInEventAsync(CancelInterestInEventCommand command)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.DeleteAsync($"events/{command.EventId}/show-interest", command);
        }

        public async Task RateEventAsync(RateEventCommand command)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PostAsync($"events/{command.EventId}/rate", command);
        }

        public async Task<EventRatingDto> GetEventRatingAsync(Guid eventId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<EventRatingDto>($"events/{eventId}/rating");
        }

        public async Task<PagedResult<EventDto>> SearchEventsAsync(SearchEvents command)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(command.Name)) queryParams.Add($"Name={HttpUtility.UrlEncode(command.Name)}");
            if (!string.IsNullOrEmpty(command.Organizer)) queryParams.Add($"Organizer={HttpUtility.UrlEncode(command.Organizer)}");
            if (command.OrganizationId != Guid.Empty) queryParams.Add($"OrganizationId={command.OrganizationId}");
            if (!string.IsNullOrEmpty(command.Category)) queryParams.Add($"Category={HttpUtility.UrlEncode(command.Category)}");
            if (!string.IsNullOrEmpty(command.State)) queryParams.Add($"State={HttpUtility.UrlEncode(command.State)}");
            if (command.Friends != null && command.Friends.Any()) queryParams.Add($"Friends={string.Join(",", command.Friends)}");
            if (!string.IsNullOrEmpty(command.FriendsEngagementType)) queryParams.Add($"FriendsEngagementType={HttpUtility.UrlEncode(command.FriendsEngagementType)}");
            if (!string.IsNullOrEmpty(command.DateFrom)) queryParams.Add($"DateFrom={HttpUtility.UrlEncode(command.DateFrom)}");
            if (!string.IsNullOrEmpty(command.DateTo)) queryParams.Add($"DateTo={HttpUtility.UrlEncode(command.DateTo)}");

            if (command.Pageable != null)
            {
                queryParams.Add($"Page={command.Pageable.Page}");
                queryParams.Add($"Size={command.Pageable.Size}");
                if (command.Pageable.Sort != null)
                {
                    if (command.Pageable.Sort.SortBy != null && command.Pageable.Sort.SortBy.Any())
                    {
                        queryParams.Add($"SortBy={HttpUtility.UrlEncode(string.Join(",", command.Pageable.Sort.SortBy))}");
                    }
                    if (!string.IsNullOrEmpty(command.Pageable.Sort.Direction))
                    {
                        queryParams.Add($"Direction={HttpUtility.UrlEncode(command.Pageable.Sort.Direction)}");
                    }
                }
            }

            var queryString = "?" + string.Join("&", queryParams);
            
            return await _httpClient.GetAsync<PagedResult<EventDto>>($"events/search{queryString}");
        }



        public async Task<HttpResponse<PagedResult<IEnumerable<EventDto>>>> SearchOrganizerEventsAsync(SearchOrganizerEvents command)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<SearchOrganizerEvents, PagedResult<IEnumerable<EventDto>>>("events/search/organizer", command);
        }

        public async Task<EventParticipantsDto> GetEventParticipantsAsync(Guid eventId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<EventParticipantsDto>($"events/{eventId}/participants");
        }

        public async Task AddEventParticipantAsync(Guid eventId, Guid studentId, string studentName)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PostAsync($"events/{eventId}/participants", new {eventId, studentId, studentName});
        }

        public async Task RemoveEventParticipantAsync(Guid eventId, Guid participantId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.DeleteAsync($"events/{eventId}/participants?participantId={participantId}");
        }

        public async Task<PagedResult<EventDto>> GetPaginatedEventsAsync(int page, int pageSize)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<PagedResult<EventDto>>($"events/paginated?page={page}&pageSize={pageSize}");
        }
        public async Task<PagedResult<EventDto>> GetMyEventsAsync(Guid organizerId, int page, int pageSize)
        {
            return await _httpClient.GetAsync<PagedResult<EventDto>>($"events/organizer/{organizerId}/paginated?page={page}&pageSize={pageSize}");
        }

        public async Task<PagedResult<EventDto>> GetUserEventsAsync(Guid userId, int page, int pageSize, string engagementType)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<PagedResult<EventDto>>($"events/users/{userId}?engagementType={engagementType}&page={page}&pageSize={pageSize}");
        }

        public async Task<PagedResult<EventDto>> GetUserEventsFeedAsync(Guid userId, int pageNumber, int pageSize, string sortBy, string direction)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var queryString = $"?pageNumber={pageNumber}&pageSize={pageSize}&sortBy={HttpUtility.UrlEncode(sortBy)}&direction={HttpUtility.UrlEncode(direction)}";
            return await _httpClient.GetAsync<PagedResult<EventDto>>($"events/users/{userId}/feed{queryString}");
        }

        public async Task ViewEventAsync(ViewEventCommand command)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PostAsync<ViewEventCommand, object>($"events/{command.EventId}/view", command);
        }

    }
}

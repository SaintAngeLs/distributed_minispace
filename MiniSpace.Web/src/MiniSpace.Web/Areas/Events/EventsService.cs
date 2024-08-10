using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
using MiniSpace.Web.Areas.Events.CommandsDto;
using MiniSpace.Web.HttpClients;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.Areas.PagedResult;
using System.Web;
using System.Linq;

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

        public Task SignUpToEventAsync(SignUpToEventCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync($"events/{command.EventId}/sign-up", command);
        }

        public Task CancelSignUpToEventAsync(CancelSignUpToEventCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"events/{command.EventId}/sign-up", command);
        }

        public Task ShowInterestInEventAsync(ShowInterestInEventCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync($"events/{command.EventId}/show-interest", command);
        }

        public Task CancelInterestInEventAsync(CancelInterestInEventCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"events/{command.EventId}/show-interest", command);
        }

        public Task RateEventAsync(RateEventCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync($"events/{command.EventId}/rate", command);
        }

        public Task<EventRatingDto> GetEventRatingAsync(Guid eventId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<EventRatingDto>($"events/{eventId}/rating");
        }

        // public Task<HttpResponse<PagedResult<IEnumerable<EventDto>>>> SearchEventsAsync(SearchEvents command)
        // {
        //     _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
        //     return _httpClient.PostAsync<SearchEvents, PagedResult<IEnumerable<EventDto>>>("events/search", command);
        // }

        public Task<HttpResponse<PagedResult<IEnumerable<EventDto>>>> SearchEventsAsync(SearchEvents command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);

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
            
            return _httpClient.GetAsync<HttpResponse<PagedResult<IEnumerable<EventDto>>>>($"events/search{queryString}");
        }


        public Task<HttpResponse<PagedResult<IEnumerable<EventDto>>>> SearchOrganizerEventsAsync(SearchOrganizerEvents command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<SearchOrganizerEvents, PagedResult<IEnumerable<EventDto>>>("events/search/organizer", command);
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

        public Task<MiniSpace.Web.Areas.PagedResult.PagedResult<EventDto>> GetPaginatedEventsAsync(int page, int pageSize)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<MiniSpace.Web.Areas.PagedResult.PagedResult<EventDto>>($"events/paginated?page={page}&pageSize={pageSize}");
        }
        public async Task<PagedResult<EventDto>> GetMyEventsAsync(Guid organizerId, int page, int pageSize)
        {
            return await _httpClient.GetAsync<MiniSpace.Web.Areas.PagedResult.PagedResult<EventDto>>($"events/organizer/{organizerId}/paginated?page={page}&pageSize={pageSize}");
        }
    }
}

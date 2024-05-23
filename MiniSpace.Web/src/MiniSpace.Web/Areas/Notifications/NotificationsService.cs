using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
using MiniSpace.Web.Data.Events;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.HttpClients;
using Blazorise;
using MiniSpace.Web.DTO.Notifications;

namespace MiniSpace.Web.Areas.Notifications
{
    public class NotificationsService: INotificationsService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;
        
        public NotificationsService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }

        public async Task<PaginatedResponseDto<NotificationDto>> GetNotificationsByUserAsync(Guid userId, int page = 1, int pageSize = 10)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var url = $"notifications/{userId}?page={page}&pageSize={pageSize}";

            // Fetch the paginated response from the server
            var response = await _httpClient.GetAsync<PaginatedResponseDto<NotificationDto>>(url);

           

            return response;
        }

        // public async Task<NotificationDto> CreateNotificationAsync(NotificationDto notification)
        // {
        //     string accessToken = await _identityService.GetAccessTokenAsync();
        //     _httpClient.SetAccessToken(accessToken);
        //     return await _httpClient.PostAsync<NotificationDto, NotificationDto>("notifications", notification);
        // }

        public async Task UpdateNotificationStatusAsync(Guid notificationId, string status)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PutAsync<object, object>($"notifications/{notificationId}/status", new { Status = status });
        }

        public async Task DeleteNotificationAsync(Guid notificationId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.DeleteAsync($"notifications/{notificationId}");
        }
    
    }
}
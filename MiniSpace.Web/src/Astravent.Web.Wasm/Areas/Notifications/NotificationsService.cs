using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.Areas.Identity;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.Data.Events;
using Astravent.Web.Wasm.DTO.Wrappers;
using Astravent.Web.Wasm.HttpClients;
using Astravent.Web.Wasm.DTO.Notifications;
using System.Collections.Concurrent;

namespace Astravent.Web.Wasm.Areas.Notifications
{
    public class NotificationsService: INotificationsService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;
        
         private static readonly ConcurrentDictionary<Guid, bool> ConnectedUsers = new();


        public NotificationsService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }

        public async Task<PaginatedResponseDto<NotificationDto>> GetNotificationsByUserAsync(Guid userId, int page = 1, int pageSize = 10, string sortOrder = "desc")
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var url = $"notifications/{userId}?page={page}&pageSize={pageSize}&sortOrder={sortOrder}";

            var response = await _httpClient.GetAsync<PaginatedResponseDto<NotificationDto>>(url);

            return response;
        }

        public async Task UpdateNotificationStatusAsync(Guid userId, Guid notificationId, string status)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var payload = new { UserId = userId, NotificationId = notificationId, Status = status };
            var url = $"notifications/{userId}/{notificationId}/status";
            await _httpClient.PutAsync<object, object>(url, payload);
        }

        public async Task UpdateNotificationStatusAsync(Guid notificationId, bool isActive)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PutAsync($"notifications/{notificationId}/status", new { IsActive = isActive }); 
        }

        public async Task DeleteNotificationAsync(Guid userId, Guid notificationId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            var payload = new { UserId = userId, NotificationId = notificationId};
            _httpClient.SetAccessToken(accessToken);
            var url = $"notifications/notification/{userId}/{notificationId}";
            await _httpClient.DeleteAsync(url, payload);
        }


        public async Task<NotificationDto> GetNotificationByIdAsync(Guid userId, Guid notificationId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var url = $"notifications/{userId}/{notificationId}";
            return await _httpClient.GetAsync<NotificationDto>(url);
        }

        public async Task<PaginatedResponseDto<NotificationDto>> GetNotificationsByUserAsync(Guid userId, int page = 1, int pageSize = 20, string sortOrder = "desc", string status = "Unread")
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var url = $"notifications/{userId}?page={page}&pageSize={pageSize}&sortOrder={sortOrder}&status={status}";

            var response = await _httpClient.GetAsync<PaginatedResponseDto<NotificationDto>>(url);

            return response;
        }
        public async Task CreateNotificationAsync(NotificationToUsersDto notification)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var url = "notifications";
            await _httpClient.PostAsync<NotificationToUsersDto, HttpResponse<NotificationToUsersDto>>(url, notification);
        }

        public void AddConnectedUser(Guid userId)
        {
            ConnectedUsers[userId] = true;
        }

        public void RemoveConnectedUser(Guid userId)
        {
            ConnectedUsers.TryRemove(userId, out _);
        }
        public Task<bool> IsUserConnectedAsync(Guid userId)
        {
            return Task.FromResult(ConnectedUsers.ContainsKey(userId));
        }

    }
}
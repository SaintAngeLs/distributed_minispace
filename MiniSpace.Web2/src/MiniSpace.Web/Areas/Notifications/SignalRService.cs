using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO.Notifications;
using System;
using System.Threading.Tasks;

namespace MiniSpace.Web.Areas.Notifications
{
    public class SignalRService : IAsyncDisposable
    {
        private HubConnection _hubConnection;
        private readonly NavigationManager _navigationManager;
        private readonly IIdentityService _identityService;
        private Guid _userId;

        public event Action<NotificationDto> NotificationReceived;

        public SignalRService(NavigationManager navigationManager, IIdentityService identityService)
        {
            _navigationManager = navigationManager;
            _identityService = identityService;
        }

        public async Task StartAsync(Guid userId)
        {
            _userId = userId;
            var hubUrl = $"http://localhost:5006/notificationHub?userId={userId}";

            Console.WriteLine($"Initializing SignalR connection to URL: {hubUrl}");

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl, options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        var token = await _identityService.GetAccessTokenAsync();
                        Console.WriteLine($"Using Access Token: {token}");
                        return token;
                    };
                })
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string>("ReceiveNotification", (jsonMessage) =>
            {
                var notification = System.Text.Json.JsonSerializer.Deserialize<NotificationDto>(jsonMessage);
                NotificationReceived?.Invoke(notification);
            });

            _hubConnection.Closed += async (error) =>
            {
                Console.WriteLine($"Connection closed due to an error: {error?.Message}");
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _hubConnection.StartAsync();
            };

            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("SignalR connection started successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
            }
        }

        public async Task StopAsync()
        {
            try
            {
                Console.WriteLine("Stopping SignalR connection...");
                await _hubConnection.StopAsync();
                Console.WriteLine("SignalR connection stopped successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping SignalR connection: {ex.Message}");
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO.Notifications;
using System;
using System.Threading.Tasks;

namespace MiniSpace.Web.Areas.Notifications
{
    public class SignalRService
    {
        private readonly HubConnection _hubConnection;
        private readonly NavigationManager _navigationManager;
        private readonly IIdentityService _identityService;

        public SignalRService(NavigationManager navigationManager, IIdentityService identityService)
        {
            _navigationManager = navigationManager;
            _identityService = identityService;
            var hubUrl = "http://localhost:5006/notificationHub";  // Correct URL for the hub

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
        }

        public async Task StartAsync()
        {
            try
            {
                Console.WriteLine("Starting SignalR connection...");
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

        public void OnNotificationReceived(Action<NotificationDto> handler)
        {
            _hubConnection.On<string>("ReceiveMessage", (jsonMessage) =>
            {
                try
                {
                    var notification = System.Text.Json.JsonSerializer.Deserialize<NotificationDto>(jsonMessage);
                    if (notification != null)
                    {
                        handler(notification);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deserializing notification message: {ex.Message}");
                }
            });
        }

    }
}

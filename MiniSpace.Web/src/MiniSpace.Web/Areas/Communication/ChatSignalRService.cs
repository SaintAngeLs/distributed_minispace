using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO.Communication;
using System;
using System.Threading.Tasks;

namespace MiniSpace.Web.Areas.Communication
{
    public class ChatSignalRService : IAsyncDisposable
    {
        private HubConnection _hubConnection;
        private readonly NavigationManager _navigationManager;
        private readonly IIdentityService _identityService;
        private Guid _userId;
        private Guid _currentChatId;
        public event Action<MessageDto> MessageReceived;
        public event Action<Guid, string> MessageStatusUpdated;

        public ChatSignalRService(NavigationManager navigationManager, IIdentityService identityService)
        {
            _navigationManager = navigationManager;
            _identityService = identityService;
        }

        public async Task StartAsync(Guid userId, Guid currentChatId)
        {
            _userId = userId;
            _currentChatId = currentChatId;
            var hubUrl = $"http://localhost:5016/chatHub?userId={userId}&chatId={currentChatId}";

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl, options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        var token = await _identityService.GetAccessTokenAsync();
                        return token;
                    };
                })
                .WithAutomaticReconnect()
                .Build();

            // Handler for receiving messages
            _hubConnection.On<string>("ReceiveMessage", (jsonMessage) =>
            {
                Console.WriteLine("ReceiveMessage event triggered");
                var message = System.Text.Json.JsonSerializer.Deserialize<MessageDto>(jsonMessage);
                MessageReceived?.Invoke(message);
            });

            // Handler for receiving message status updates
            _hubConnection.On<string>("ReceiveMessageStatusUpdate", (jsonStatusUpdate) =>
            {
                try
                {
                    Console.WriteLine("ReceiveMessageStatusUpdate event triggered: " + jsonStatusUpdate);

                    // Deserialize the JSON to a strongly-typed object
                    var statusUpdate = System.Text.Json.JsonSerializer.Deserialize<MessageStatusUpdateDto>(jsonStatusUpdate);
                    
                    var chatId = statusUpdate.ChatId;
                    var messageId = Guid.Parse(statusUpdate.MessageId);
                    var status = statusUpdate.Status;

                    Console.WriteLine($"Chat ID: {chatId}, Message ID: {messageId}, Status: {status}");

                    // Ensure the update is only for the current chat
                    if (chatId == _currentChatId.ToString())
                    {
                        MessageStatusUpdated?.Invoke(messageId, status);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error processing status update: " + ex.Message);
                }
            });

            // Log connection state changes
            _hubConnection.Closed += async (error) =>
            {
                Console.WriteLine("SignalR connection closed. Attempting to reconnect...");
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _hubConnection.StartAsync();
            };

            _hubConnection.Reconnected += connectionId =>
            {
                Console.WriteLine($"Reconnected to SignalR with connection ID: {connectionId}");
                return Task.CompletedTask;
            };

            _hubConnection.Reconnecting += error =>
            {
                Console.WriteLine("SignalR connection lost, reconnecting...");
                return Task.CompletedTask;
            };

            // Start the SignalR connection
            await _hubConnection.StartAsync();
            Console.WriteLine("SignalR connection started successfully");
        }

        public async Task StopAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
                Console.WriteLine("SignalR connection stopped.");
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
                Console.WriteLine("SignalR connection disposed.");
            }
        }
    }
}

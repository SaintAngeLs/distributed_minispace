using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
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
    private bool _disposed;

    public event Action<MessageDto> MessageReceived;
    public event Action<Guid, string> MessageStatusUpdated;
    public event Action<string, bool> TypingNotificationReceived;
    public event Action<bool> ConnectionChanged;

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
                    if (_disposed)
                    {
                        return null; // Circuit is disposed, return null to avoid further calls.
                    }

                    try
                    {
                        var token = await _identityService.GetAccessTokenAsync();
                        return token;
                    }
                    catch (JSDisconnectedException)
                    {
                        // Handle the JS interop disconnection gracefully
                        return null;
                    }
                };
            })
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<string>("ReceiveMessage", (jsonMessage) =>
        {
            var message = System.Text.Json.JsonSerializer.Deserialize<MessageDto>(jsonMessage);
            MessageReceived?.Invoke(message);
        });

        _hubConnection.On<string>("ReceiveMessageStatusUpdate", (jsonStatusUpdate) =>
        {
            var statusUpdate = System.Text.Json.JsonSerializer.Deserialize<MessageStatusUpdateDto>(jsonStatusUpdate);
            var chatId = statusUpdate.ChatId;
            var messageId = Guid.Parse(statusUpdate.MessageId);
            var status = statusUpdate.Status;

            if (chatId == _currentChatId.ToString())
            {
                MessageStatusUpdated?.Invoke(messageId, status);
            }
        });

        _hubConnection.On<string, bool>("ReceiveTypingNotification", (userId, isTyping) =>
        {
            TypingNotificationReceived?.Invoke(userId, isTyping);
        });

        _hubConnection.Reconnecting += (error) =>
        {
            ConnectionChanged?.Invoke(false);
            return Task.CompletedTask;
        };

        _hubConnection.Reconnected += (connectionId) =>
        {
            ConnectionChanged?.Invoke(true);
            return Task.CompletedTask;
        };

        _hubConnection.Closed += (error) =>
        {
            ConnectionChanged?.Invoke(false);
            return Task.CompletedTask;
        };

        if (!_disposed)
        {
            await _hubConnection.StartAsync();
            ConnectionChanged?.Invoke(true);
        }
    }

    public async Task SendTypingNotificationAsync(bool isTyping)
    {
        if (_hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.InvokeAsync("SendTypingNotification", _currentChatId.ToString(), _userId.ToString(), isTyping);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.DisposeAsync();
        }
        _disposed = true;
    }
}


}

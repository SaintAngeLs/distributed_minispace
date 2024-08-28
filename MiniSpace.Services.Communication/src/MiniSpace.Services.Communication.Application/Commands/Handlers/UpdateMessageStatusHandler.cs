using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Events;
using MiniSpace.Services.Communication.Application.Hubs;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class UpdateMessageStatusHandler : ICommandHandler<UpdateMessageStatus>
    {
        private readonly IUserChatsRepository _userChatsRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<UpdateMessageStatusHandler> _logger;
        private readonly IHubContext<ChatHub> _hubContext;

        public UpdateMessageStatusHandler(
            IUserChatsRepository userChatsRepository,
            IMessageBroker messageBroker,
            ILogger<UpdateMessageStatusHandler> logger,
            IHubContext<ChatHub> hubContext)
        {
            _userChatsRepository = userChatsRepository;
            _messageBroker = messageBroker;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(UpdateMessageStatus command, CancellationToken cancellationToken)
        {
            // Retrieve the chat using the GetByChatIdAsync method
            var chat = await _userChatsRepository.GetByChatIdAsync(command.ChatId);

            if (chat == null)
            {
                _logger.LogWarning($"Chat with ID {command.ChatId} not found.");
                return;  // Exit if the chat is not found
            }

            // Find the message by its ID within the chat
            var message = chat.Messages.Find(m => m.Id == command.MessageId);
            if (message == null)
            {
                _logger.LogWarning($"Message with ID {command.MessageId} not found in chat with ID {command.ChatId}.");
                return;  // Exit if the message is not found
            }

            // Update the message status based on the command
            switch (command.Status)
            {
                case "Read":
                    message.MarkAsRead();
                    break;
                case "Unread":
                    message.MarkAsUnread();
                    break;
                case "Deleted":
                    message.MarkAsDeleted();
                    break;
            }

            // Update the user chats to reflect the status change
            var userChats = await _userChatsRepository.GetByUserIdAsync(chat.ParticipantIds.FirstOrDefault());
            await _userChatsRepository.UpdateAsync(userChats);

           // Publish the event to notify other systems
            await _messageBroker.PublishAsync(new MessageStatusUpdated(command.ChatId, command.MessageId, command.Status));

            // Log the status update
            _logger.LogInformation($"Message status updated: ChatId={command.ChatId}, MessageId={command.MessageId}, Status={command.Status}");

            // Notify the chat participants of the status change via SignalR
            await ChatHub.SendMessageStatusUpdate(_hubContext, command.ChatId.ToString(), command.MessageId, command.Status, _logger);

             
        }
    }
}

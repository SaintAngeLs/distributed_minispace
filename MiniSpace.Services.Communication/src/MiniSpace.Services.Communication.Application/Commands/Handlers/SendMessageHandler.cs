using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Events;
using MiniSpace.Services.Communication.Application.Hubs;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Core.Entities;
using MiniSpace.Services.Communication.Core.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using MiniSpace.Services.Communication.Application.Dto;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class SendMessageHandler : ICommandHandler<SendMessage>
    {
        private readonly IUserChatsRepository _userChatsRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<SendMessageHandler> _logger;
        private readonly IHubContext<ChatHub> _hubContext;

        public SendMessageHandler(
            IUserChatsRepository userChatsRepository,
            IMessageBroker messageBroker,
            ILogger<SendMessageHandler> logger,
            IHubContext<ChatHub> hubContext)
        {
            _userChatsRepository = userChatsRepository;
            _messageBroker = messageBroker;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task HandleAsync(SendMessage command, CancellationToken cancellationToken)
        {
            // Retrieve the chat from the sender's chat list
            var senderChats = await _userChatsRepository.GetByUserIdAsync(command.SenderId);
            var chat = senderChats?.GetChatById(command.ChatId);

            if (chat == null)
            {
                _logger.LogWarning($"Chat with id {command.ChatId} not found for user with id {command.SenderId}");
                return;
            }

            // Create the message to be sent
            var message = new Message(
                chatId: command.ChatId,
                senderId: command.SenderId,
                receiverId: Guid.Empty, // Unused in this context
                content: command.Content,
                type: Enum.Parse<MessageType>(command.MessageType)
            );

            _logger.LogInformation($"Sending message with id {message.Id} to chat with id {command.ChatId}");

            chat.AddMessage(message);

            // Save the updated chat and message to the database
            foreach (var participantId in chat.ParticipantIds)
            {
                var participantChats = await _userChatsRepository.GetByUserIdAsync(participantId) ?? new UserChats(participantId);
                var participantChat = participantChats.GetChatById(command.ChatId) ?? new Chat(chat.ParticipantIds);
                participantChat.AddMessage(message);
                await _userChatsRepository.UpdateAsync(participantChats);
            }

            // Use the message ID from the database for the MessageDto
            var messageDto = new MessageDto
            {
                Id = message.Id, // Use the ID from the database
                ChatId = command.ChatId,
                SenderId = command.SenderId,
                Content = command.Content,
                Timestamp = message.Timestamp // Assuming Message has a Timestamp property
            };

            // Notify the participant via SignalR
            await ChatHub.SendMessageToUser(_hubContext, command.SenderId.ToString(), messageDto, _logger);

            // Publish the MessageSent event for further processing
            await _messageBroker.PublishAsync(new MessageSent(
                chatId: command.ChatId,
                messageId: message.Id,
                senderId: command.SenderId,
                content: command.Content
            ));
        }
    }
}

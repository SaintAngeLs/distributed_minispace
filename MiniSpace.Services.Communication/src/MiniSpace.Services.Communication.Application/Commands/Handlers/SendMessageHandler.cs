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
using System.Collections.Generic;

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
                _logger.LogWarning($"Chat with id {command.ChatId} not found for user with id {command.SenderId}. Checking if it exists for other participants...");

                // If the chat is not found, check if it exists for any other participant
                chat = await FindChatInAllParticipants(command.SenderId, command.ChatId);

                if (chat == null)
                {
                    _logger.LogInformation($"No existing chat found. Creating a new chat for the participants.");
                    // If no chat is found at all, create a new one
                    // If creating a new chat
                    chat = new Chat(command.ChatId, new List<Guid> { command.SenderId }, new List<Message>());

                    await CreateOrUpdateChatForAllParticipants(chat);
                }
                else
                {
                    // Restore the chat for the sender
                    senderChats ??= new UserChats(command.SenderId);
                    senderChats.AddChat(chat);
                    await _userChatsRepository.AddOrUpdateAsync(senderChats);
                }
            }

            // Create the message to be sent
            var message = new Message(
                chatId: chat.Id,
                senderId: command.SenderId,
                receiverId: Guid.Empty, // Unused in this context
                content: command.Content,
                type: Enum.Parse<MessageType>(command.MessageType)
            );

            _logger.LogInformation($"Sending message with id {message.Id} to chat with id {chat.Id}");

            chat.AddMessage(message);

            // Save the updated chat and message to the database
            await UpdateChatForAllParticipants(chat);

            // Use the message ID from the database for the MessageDto
            var messageDto = new MessageDto
            {
                Id = message.Id, // Use the ID from the database
                ChatId = chat.Id,
                SenderId = command.SenderId,
                Content = command.Content,
                Timestamp = message.Timestamp // Assuming Message has a Timestamp property
            };
            

            // Notify the participant via SignalR
            await ChatHub.SendMessageToUser(_hubContext, command.SenderId.ToString(), messageDto, _logger);

            // Publish the MessageSent event for further processing
            await _messageBroker.PublishAsync(new MessageSent(
                chatId: chat.Id,
                messageId: message.Id,
                senderId: command.SenderId,
                content: command.Content
            ));
        }

        private async Task<Chat> FindChatInAllParticipants(Guid senderId, Guid chatId)
        {
            // Go through all participant chats to find if the chat already exists
            var participantIds = await _userChatsRepository.GetParticipantIdsByChatIdAsync(chatId);

            foreach (var participantId in participantIds)
            {
                if (participantId == senderId) continue;

                var participantChats = await _userChatsRepository.GetByUserIdAsync(participantId);
                var existingChat = participantChats?.GetChatById(chatId);

                if (existingChat != null)
                {
                    _logger.LogInformation($"Chat found for another participant with id {participantId}. Restoring chat for sender.");
                    return existingChat;
                }
            }

            return null;
        }

        private async Task CreateOrUpdateChatForAllParticipants(Chat chat)
        {
            foreach (var participantId in chat.ParticipantIds)
            {
                var participantChats = await _userChatsRepository.GetByUserIdAsync(participantId) ?? new UserChats(participantId);
                participantChats.AddChat(chat);
                await _userChatsRepository.AddOrUpdateAsync(participantChats);
            }
        }

        private async Task UpdateChatForAllParticipants(Chat chat)
        {
            foreach (var participantId in chat.ParticipantIds)
            {
                var participantChats = await _userChatsRepository.GetByUserIdAsync(participantId) ?? new UserChats(participantId);
                var participantChat = participantChats.GetChatById(chat.Id) ?? chat;
                participantChat.AddMessage(chat.Messages.Last());
                await _userChatsRepository.AddOrUpdateAsync(participantChats);
            }
        }

    }
}

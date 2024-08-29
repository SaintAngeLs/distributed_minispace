using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Events;
using MiniSpace.Services.Communication.Core.Entities;
using MiniSpace.Services.Communication.Core.Repositories;

namespace MiniSpace.Services.Communication.Application.Services
{
    public class CommunicationService : ICommunicationService
    {
        private readonly IUserChatsRepository _userChatsRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<CommunicationService> _logger;

        public CommunicationService(IUserChatsRepository userChatsRepository, IMessageBroker messageBroker, ILogger<CommunicationService> logger)
        {
            _userChatsRepository = userChatsRepository;
            _messageBroker = messageBroker;
            _logger = logger;
        }

        public async Task<Guid> CreateChatAsync(Guid chatId, List<Guid> participantIds, string chatName = null)
        {
            _logger.LogInformation($"Creating chat with ID: {chatId}");

            // Check if the chat already exists
            Chat existingChat = null;
            foreach (var participantId in participantIds)
            {
                var userChats = await _userChatsRepository.GetByUserIdAsync(participantId);
                if (userChats != null)
                {
                    existingChat = userChats.Chats.Find(chat =>
                        chat.ParticipantIds.Count == participantIds.Count &&
                        chat.ParticipantIds.All(pid => participantIds.Contains(pid))
                    );

                    if (existingChat != null)
                    {
                        _logger.LogWarning($"Chat with ID: {existingChat.Id} already exists. Returning existing chat ID.");
                        return existingChat.Id;
                    }
                }
            }

            // Create a new chat if none exists
            var newChat = new Chat(chatId, participantIds, new List<Message>());
            foreach (var participantId in participantIds)
            {
                var userChats = await _userChatsRepository.GetByUserIdAsync(participantId) ?? new UserChats(participantId);
                userChats.AddChat(newChat);
                await _userChatsRepository.AddOrUpdateAsync(userChats);
            }

            await _messageBroker.PublishAsync(new ChatCreated(newChat.Id, participantIds));

            _logger.LogInformation($"New chat created with ID: {newChat.Id}");
            return newChat.Id;
        }


        public async Task UpdateMessageStatusAsync(Guid chatId, Guid messageId, string status)
        {
            // Retrieve the chat using the GetByChatIdAsync method
            var chat = await _userChatsRepository.GetByChatIdAsync(chatId);
            if (chat == null)
            {
                _logger.LogWarning($"Chat with ID {chatId} not found.");
                return;
            }

            var message = chat.Messages.FirstOrDefault(m => m.Id == messageId);
            if (message == null)
            {
                _logger.LogWarning($"Message with ID {messageId} not found in chat with ID {chatId}.");
                return;
            }

            switch (status)
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
                default:
                    _logger.LogWarning($"Unsupported status '{status}' for message ID {messageId} in chat ID {chatId}.");
                    return;
            }

            foreach (var participantId in chat.ParticipantIds)
            {
                var userChats = await _userChatsRepository.GetByUserIdAsync(participantId);
                if (userChats != null)
                {
                    var existingChat = userChats.GetChatById(chatId);
                    if (existingChat != null)
                    {
                        var messageToUpdate = existingChat.Messages.FirstOrDefault(m => m.Id == messageId);
                        if (messageToUpdate != null)
                        {
                            _logger.LogInformation($"Updating message status for chat ID {chatId} and message ID {messageId} to {status}");
                            messageToUpdate.MarkAsRead(); 
                        }
                        _logger.LogInformation($"Updating chat for participant {participantId} with chat ID {chatId}");
                        await _userChatsRepository.UpdateAsync(userChats); 
                    }
                }
            }

            _logger.LogInformation($"Message status updated: ChatId={chatId}, MessageId={messageId}, Status={status}");

            await _messageBroker.PublishAsync(new MessageStatusUpdated(chatId, messageId, status));
        }

    }
}

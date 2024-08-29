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
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Dto;

namespace MiniSpace.Services.Communication.Application.Services
{
    public interface ICommunicationService
    {
        Task<Guid> CreateChatAsync(Guid chatId, List<Guid> participantIds, string chatName = null);
        Task UpdateMessageStatusAsync(Guid chatId, Guid messageId, string status);
        // Task<IEnumerable<ChatDto>> GetUserChatsAsync(Guid userId);
        // Task<ChatDto> GetChatByIdAsync(Guid chatId);
        // Task SendMessageAsync(SendMessage command);
    }
}

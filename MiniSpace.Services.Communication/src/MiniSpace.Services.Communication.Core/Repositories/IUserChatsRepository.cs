using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Communication.Core.Entities;

namespace MiniSpace.Services.Communication.Core.Repositories
{
    public interface IUserChatsRepository
    {
        Task<UserChats> GetByUserIdAsync(Guid userId);
        Task AddAsync(UserChats userChats);
        Task UpdateAsync(UserChats userChats);
        Task AddOrUpdateAsync(UserChats userChats);
        Task DeleteAsync(Guid userId);
        Task<bool> ChatExistsAsync(Guid userId, Guid chatId);
        Task AddChatAsync(Guid userId, Chat chat);
        Task DeleteChatAsync(Guid userId, Guid chatId);
        Task<Chat> GetByChatIdAsync(Guid chatId);
    }
}

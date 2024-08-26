using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Communication.Core.Entities;

namespace MiniSpace.Services.Communication.Core.Repositories
{
    public interface IOrganizationChatsRepository
    {
        Task<OrganizationChats> GetByOrganizationIdAsync(Guid organizationId);
        Task AddAsync(OrganizationChats organizationChats);
        Task UpdateAsync(OrganizationChats organizationChats);
        Task AddOrUpdateAsync(OrganizationChats organizationChats);
        Task DeleteAsync(Guid organizationId);
        Task<bool> ChatExistsAsync(Guid organizationId, Guid chatId);
        Task AddChatAsync(Guid organizationId, Chat chat);
        Task DeleteChatAsync(Guid organizationId, Guid chatId);
    }
}

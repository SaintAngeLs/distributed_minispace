using MiniSpace.Services.Communication.Core.Entities;
using MiniSpace.Services.Communication.Core.Repositories;
using MiniSpace.Services.Communication.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using Paralax.Persistence.MongoDB;
using System;
using System.Threading.Tasks;

namespace MiniSpace.Services.Communication.Infrastructure.Mongo.Repositories
{
    public class OrganizationChatsRepository : IOrganizationChatsRepository
    {
        private readonly IMongoRepository<OrganizationChatsDocument, Guid> _repository;

        public OrganizationChatsRepository(IMongoRepository<OrganizationChatsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<OrganizationChats> GetByOrganizationIdAsync(Guid organizationId)
        {
            var document = await _repository.GetAsync(x => x.OrganizationId == organizationId);
            return document?.AsEntity();
        }

        public async Task AddAsync(OrganizationChats organizationChats)
        {
            await _repository.AddAsync(organizationChats.AsDocument());
        }

        public async Task UpdateAsync(OrganizationChats organizationChats)
        {
            await _repository.UpdateAsync(organizationChats.AsDocument());
        }

        public async Task AddOrUpdateAsync(OrganizationChats organizationChats)
        {
            var existingDocument = await _repository.GetAsync(x => x.OrganizationId == organizationChats.OrganizationId);
            if (existingDocument == null)
            {
                await AddAsync(organizationChats);
            }
            else
            {
                await UpdateAsync(organizationChats);
            }
        }

        public async Task DeleteAsync(Guid organizationId)
        {
            await _repository.DeleteAsync(x => x.OrganizationId == organizationId);
        }

        public async Task<bool> ChatExistsAsync(Guid organizationId, Guid chatId)
        {
            var document = await _repository.GetAsync(x => x.OrganizationId == organizationId && x.Chats.Any(c => c.Id == chatId));
            return document != null;
        }

        public async Task AddChatAsync(Guid organizationId, Chat chat)
        {
            var organizationChats = await GetByOrganizationIdAsync(organizationId) ?? new OrganizationChats(organizationId);
            organizationChats.AddChat(chat);
            await AddOrUpdateAsync(organizationChats);
        }

        public async Task DeleteChatAsync(Guid organizationId, Guid chatId)
        {
            var organizationChats = await GetByOrganizationIdAsync(organizationId);
            if (organizationChats != null)
            {
                var chat = organizationChats.GetChatById(chatId);
                if (chat != null)
                {
                    organizationChats.Chats.Remove(chat);
                    await UpdateAsync(organizationChats);
                }
            }
        }
    }
}

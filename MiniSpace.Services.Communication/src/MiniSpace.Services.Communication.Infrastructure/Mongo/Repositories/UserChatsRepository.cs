using MiniSpace.Services.Communication.Core.Entities;
using MiniSpace.Services.Communication.Core.Repositories;
using MiniSpace.Services.Communication.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using Paralax.Persistence.MongoDB;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Communication.Infrastructure.Mongo.Repositories
{
    public class UserChatsRepository : IUserChatsRepository
    {
        private readonly IMongoRepository<UserChatsDocument, Guid> _repository;

        public UserChatsRepository(IMongoRepository<UserChatsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<UserChats> GetByUserIdAsync(Guid userId)
        {
            var document = await _repository.GetAsync(x => x.UserId == userId);
            return document?.AsEntity();
        }

        public async Task<Chat> GetByChatIdAsync(Guid chatId)
        {
            var document = await _repository.Collection.Find(x => x.Chats.Any(c => c.Id == chatId)).FirstOrDefaultAsync();
            var chatDocument = document?.Chats.FirstOrDefault(c => c.Id == chatId);
            return chatDocument?.AsEntity();
        }

        public async Task AddAsync(UserChats userChats)
        {
            await _repository.AddAsync(userChats.AsDocument());
        }

        public async Task UpdateAsync(UserChats userChats)
        {
            var filter = Builders<UserChatsDocument>.Filter.Eq(doc => doc.UserId, userChats.UserId);
            var update = Builders<UserChatsDocument>.Update
                .Set(doc => doc.Chats, userChats.Chats.Select(chat => chat.AsDocument()).ToList());

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task AddOrUpdateAsync(UserChats userChats)
        {
            var existingDocument = await _repository.GetAsync(x => x.UserId == userChats.UserId);
            if (existingDocument == null)
            {
                await AddAsync(userChats);
            }
            else
            {
                await UpdateAsync(userChats);
            }
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _repository.DeleteAsync(x => x.UserId == userId);
        }

        public async Task<bool> ChatExistsAsync(Guid userId, Guid chatId)
        {
            var document = await _repository.GetAsync(x => x.UserId == userId && x.Chats.Any(c => c.Id == chatId));
            return document != null;
        }

        public async Task AddChatAsync(Guid userId, Chat chat)
        {
            var userChats = await GetByUserIdAsync(userId) ?? new UserChats(userId);
            userChats.AddChat(chat);
            await AddOrUpdateAsync(userChats);
        }

        public async Task DeleteChatAsync(Guid userId, Guid chatId)
        {
            var userChats = await GetByUserIdAsync(userId);
            if (userChats == null)
            {
                return;
            }
            var chat = userChats.GetChatById(chatId);
            if (chat == null)
            {
                return;
            }
            userChats.Chats.Remove(chat);
            await UpdateAsync(userChats);
        }

        public async Task<List<Guid>> GetParticipantIdsByChatIdAsync(Guid chatId)
        {
            var document = await _repository.Collection
                .Find(x => x.Chats.Any(c => c.Id == chatId))
                .FirstOrDefaultAsync();

            var chatDocument = document?.Chats.FirstOrDefault(c => c.Id == chatId);
            return chatDocument?.ParticipantIds ?? new List<Guid>();
        }

    }
}

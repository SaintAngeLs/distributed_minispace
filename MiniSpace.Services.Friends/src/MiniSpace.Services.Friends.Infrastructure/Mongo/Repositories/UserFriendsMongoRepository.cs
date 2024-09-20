using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Repositories
{
    public class UserFriendsMongoRepository : IUserFriendsRepository
    {
        private readonly IMongoRepository<UserFriendsDocument, Guid> _repository;

        public UserFriendsMongoRepository(IMongoRepository<UserFriendsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<UserFriends> GetAsync(Guid userId)
        {
            var document = await _repository.GetAsync(f => f.UserId == userId);
            return document?.AsEntity();
        }

        public async Task<IEnumerable<UserFriends>> GetAllAsync()
        {
            var documents = await _repository.FindAsync(_ => true);
            return documents.Select(d => d.AsEntity());
        }

        public async Task AddAsync(UserFriends userFriends)
        {
            var document = userFriends.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task UpdateAsync(UserFriends userFriends)
        {
            var document = userFriends.AsDocument();
            await _repository.UpdateAsync(document);
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _repository.DeleteAsync(userId);
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            var exists = await _repository.ExistsAsync(f => f.UserId == userId);
            return exists;
        }

        public async Task<IEnumerable<Friend>> GetFriendsAsync(Guid userId)
        {
            var document = await _repository.GetAsync(f => f.UserId == userId);
            return document?.Friends.Select(f => f.AsEntity()) ?? Enumerable.Empty<Friend>();
        }

        public async Task AddOrUpdateAsync(UserFriends userFriends)
        {
            var exists = await ExistsAsync(userFriends.UserId);
            if (exists)
            {
                await UpdateAsync(userFriends);
            }
            else
            {
                await AddAsync(userFriends);
            }
        }

        public async Task RemoveFriendAsync(Guid userId, Guid friendId)
        {
            var document = await _repository.GetAsync(f => f.UserId == userId);
            if (document != null)
            {
                var friend = document.Friends.FirstOrDefault(f => f.FriendId == friendId);
                if (friend != null)
                {
                    document.Friends.Remove(friend);
                    await _repository.UpdateAsync(document);
                }
            }
        }
    }
}

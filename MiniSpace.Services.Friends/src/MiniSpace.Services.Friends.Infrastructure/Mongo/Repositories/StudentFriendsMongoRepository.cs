using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using MongoDB.Driver;

public class UserFriendsMongoRepository : IUserFriendsRepository
{
    private readonly IMongoRepository<UserFriendsDocument, Guid> _repository;

    public UserFriendsMongoRepository(IMongoRepository<UserFriendsDocument, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<UserFriends> GetAsync(Guid studentId)
    {
        var document = await _repository.GetAsync(studentId);
        return document?.AsEntity();
    }

    public async Task<IEnumerable<UserFriends>> GetAllAsync()
    {
        var documents = await _repository.FindAsync(_ => true);
        return documents.Select(doc => doc.AsEntity());
    }

    public async Task AddAsync(UserFriends studentFriends)
    {
        var document = studentFriends.AsDocument();
        await _repository.AddAsync(document);
    }

    public async Task UpdateAsync(UserFriends studentFriends)
    {
        var document = studentFriends.AsDocument();
        await _repository.UpdateAsync(document);
    }

    public async Task DeleteAsync(Guid studentId)
    {
        await _repository.DeleteAsync(studentId);
    }

    public async Task<bool> ExistsAsync(Guid studentId)
    {
        var document = await _repository.GetAsync(studentId);
        return document != null;
    }

    public async Task<IEnumerable<Friend>> GetFriendsAsync(Guid studentId)
    {
        var documents = await _repository.FindAsync(doc => doc.UserId == studentId);
        if (documents == null || !documents.Any())
        {
            return Enumerable.Empty<Friend>();
        }

        var document = documents.First(); 
        return document.Friends.Select(doc => new Friend(
            doc.UserId,
            doc.FriendId,
            doc.CreatedAt,
            doc.State)).ToList();
    }
    
    public async Task AddOrUpdateAsync(UserFriends studentFriends)
    {
        var filter = Builders<UserFriendsDocument>.Filter.Eq(doc => doc.UserId, studentFriends.UserId);
        var update = Builders<UserFriendsDocument>.Update
            .SetOnInsert(doc => doc.UserId, studentFriends.UserId) 
            .Set(doc => doc.Id, studentFriends.UserId) 
            .AddToSetEach(doc => doc.Friends, studentFriends.Friends.Select(f => f.AsDocument()));

        var options = new UpdateOptions { IsUpsert = true };
        await _repository.Collection.UpdateOneAsync(filter, update, options);
    }

    public async Task RemoveFriendAsync(Guid studentId, Guid friendId)
    {
        var filter = Builders<UserFriendsDocument>.Filter.Eq(doc => doc.UserId, studentId);
        var update = Builders<UserFriendsDocument>.Update.PullFilter(doc => doc.Friends, Builders<FriendDocument>.Filter.Eq("FriendId", friendId));

        await _repository.Collection.UpdateOneAsync(filter, update);
    }
}

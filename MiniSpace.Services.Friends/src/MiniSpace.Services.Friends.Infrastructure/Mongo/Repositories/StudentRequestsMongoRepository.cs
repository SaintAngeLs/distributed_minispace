using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Repositories
{
    public class UserRequestsMongoRepository : IUserRequestsRepository
    {
        private readonly IMongoRepository<UserRequestsDocument, Guid> _repository;

        public UserRequestsMongoRepository(IMongoRepository<UserRequestsDocument, Guid> repository)
        {
            _repository = repository;
        }
     
        public async Task<UserRequests> GetAsync(Guid studentId)
        {
            var document = await _repository.FindAsync(doc => doc.UserId == studentId);
            var studentRequestDocument = document.SingleOrDefault();
            if (studentRequestDocument == null)
            {
                return null;
            }

            var entity = studentRequestDocument.AsEntity();
            var json = JsonSerializer.Serialize(entity, new JsonSerializerOptions { WriteIndented = true });

            return entity;
        }


        public async Task<IEnumerable<UserRequests>> GetAllAsync()
        {
            var documents = await _repository.FindAsync(_ => true);
            return documents.Select(doc => doc.AsEntity()); 
        }

        public async Task AddAsync(UserRequests studentRequests)
        {
            var document = studentRequests.AsDocument(); 
            await _repository.AddAsync(document);
        }

        public async Task UpdateAsync(UserRequests studentRequests)
        {
            var document = studentRequests.AsDocument();
            await _repository.UpdateAsync(document);
        }

        public async Task UpdateAsync(Guid studentId, IEnumerable<FriendRequest> updatedFriendRequests)
        {
            var document = await _repository.FindAsync(doc => doc.UserId == studentId);
            var studentRequestDocument = document.SingleOrDefault();
            if (studentRequestDocument == null)
            {
                return; // Consider handling this case appropriately, possibly by adding a new document.
            }
            studentRequestDocument.FriendRequests = updatedFriendRequests.Select(fr => fr.AsDocument()).ToList();

            var filter = Builders<UserRequestsDocument>.Filter.Eq(doc => doc.UserId, studentRequestDocument.UserId);
            var update = Builders<UserRequestsDocument>.Update.Set(doc => doc.FriendRequests, studentRequestDocument.FriendRequests);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(Guid studentId)
        {
            var documents = await _repository.FindAsync(doc => doc.UserId == studentId);
            var document = documents.SingleOrDefault();
            if (document != null)
            {
                await _repository.DeleteAsync(document.Id);
            }
        }

        public async Task RemoveFriendRequestAsync(Guid requesterId, Guid friendId)
        {
            var filter = Builders<UserRequestsDocument>.Filter.Eq(doc => doc.UserId, requesterId) &
                        Builders<UserRequestsDocument>.Filter.Or(
                            Builders<UserRequestsDocument>.Filter.ElemMatch(doc => doc.FriendRequests, Builders<FriendRequestDocument>.Filter.Eq(fr => fr.InviterId, friendId)),
                            Builders<UserRequestsDocument>.Filter.ElemMatch(doc => doc.FriendRequests, Builders<FriendRequestDocument>.Filter.Eq(fr => fr.InviteeId, friendId))
                        );

            var update = Builders<UserRequestsDocument>.Update.PullFilter(doc => doc.FriendRequests,
                Builders<FriendRequestDocument>.Filter.Or(
                    Builders<FriendRequestDocument>.Filter.Eq(fr => fr.InviterId, friendId),
                    Builders<FriendRequestDocument>.Filter.Eq(fr => fr.InviteeId, friendId)
                ));

            await _repository.Collection.UpdateOneAsync(filter, update);
        }
    }
}

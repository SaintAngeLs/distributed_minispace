using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Repositories
{
    public class FriendRequestMongoRepository : IFriendRequestRepository
    {
        private readonly IMongoRepository<FriendRequestDocument, Guid> _repository;

        public FriendRequestMongoRepository(IMongoRepository<FriendRequestDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<FriendRequest> GetAsync(Guid id)
        {
            var document = await _repository.GetAsync(id);
            return document?.AsEntity();
        }

        public async Task AddAsync(FriendRequest friendRequest)
        {
            var document = friendRequest.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task UpdateAsync(FriendRequest friendRequest)
        {
            var documentToUpdate = friendRequest.AsDocument();

            documentToUpdate.State = friendRequest.State;

            await _repository.UpdateAsync(documentToUpdate);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<FriendRequest>> GetFriendRequestsByUser(Guid userId)
        {
            var documents = await _repository.FindAsync(fr => fr.InviterId == userId || fr.InviteeId == userId);
            return documents.Select(doc => doc.AsEntity());
        }

        public async Task<FriendRequest> FindByInviterAndInvitee(Guid inviterId, Guid inviteeId)
        {
            var document = await _repository.FindAsync(fr => fr.InviterId == inviterId && fr.InviteeId == inviteeId);
            return document.FirstOrDefault()?.AsEntity();
        }
    }
}

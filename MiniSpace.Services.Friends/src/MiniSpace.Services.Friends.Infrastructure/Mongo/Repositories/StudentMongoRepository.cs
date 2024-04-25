using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Repositories
{
    public class FriendMongoRepository : IFriendRepository
    {
        private readonly IMongoRepository<FriendDocument, Guid> _repository;
        private readonly IMongoRepository<FriendRequestDocument, Guid> _friendRequestRepository;

        public FriendMongoRepository(IMongoRepository<FriendDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Friend> GetAsync(Guid id)
        {
            var friendDocument = await _repository.GetAsync(f => f.Id == id);
            return friendDocument?.AsEntity();
        }

         public Task AddFriendAsync(Guid requesterId, Guid friendId)
        {
            var friend = new FriendDocument
            {
                Id = Guid.NewGuid(),
                StudentId = requesterId,
                FriendId = friendId,
                CreatedAt = DateTime.UtcNow,
                State = FriendState.Requested
            };
            return _repository.AddAsync(friend);
        }

        public async Task UpdateAsync(Friend friend)
        {
            await _repository.UpdateAsync(friend.AsDocument());
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var friend = await _repository.GetAsync(f => f.Id == id);
            return friend != null;
        }

        public async Task<List<Friend>> GetFriendsAsync(Guid studentId)
        {
            var documents = await _repository.FindAsync(f => f.StudentId == studentId);
            return documents?.Select(d => d.AsEntity()).ToList();
        }

        public async Task<Friend> GetFriendshipAsync(Guid requesterId, Guid friendId)
        {
            var document = await _repository.GetAsync(f => f.StudentId == requesterId && f.FriendId == friendId);
            return document?.AsEntity();
        }

        public async Task<bool> IsFriendAsync(Guid studentId, Guid potentialFriendId)
        {
            var friend = await _repository.GetAsync(f => f.StudentId == studentId && f.FriendId == potentialFriendId);
            return friend != null;
        }

        public async Task RemoveFriendAsync(Guid requesterId, Guid friendId)
        {
            var friend = await _repository.GetAsync(f => f.StudentId == requesterId && f.FriendId == friendId);
            if (friend != null)
            {
                await _repository.DeleteAsync(friend.Id);
            }
        }

        public async Task AcceptFriendInvitationAsync(Guid requesterId, Guid friendId)
        {
            var friend = await _repository.GetAsync(f => f.StudentId == requesterId && f.FriendId == friendId);
            if (friend != null)
            {
                friend.State = FriendState.Accepted;
                await _repository.UpdateAsync(friend);
            }
        }

        public async Task DeclineFriendInvitationAsync(Guid requesterId, Guid friendId)
        {
            var friend = await _repository.GetAsync(f => f.StudentId == requesterId && f.FriendId == friendId);
            if (friend != null)
            {
                friend.State = FriendState.Declined;
                await _repository.UpdateAsync(friend);
            }
        }

        public async Task InviteFriendAsync(Guid inviterId, Guid inviteeId)
        {
            var newFriend = new FriendDocument
            {
                StudentId = inviterId,
                FriendId = inviteeId,
                CreatedAt = DateTime.UtcNow,
                State = FriendState.Requested
            };
            await _repository.AddAsync(newFriend);
        }

        public Task AddRequestAsync(FriendRequest request)
        {
            var requestDocument = request.AsDocument();
            return _friendRequestRepository.AddAsync(requestDocument);
        }

        public Task AddInvitationAsync(FriendRequest invitation)
        {
            var invitationDocument = invitation.AsDocument();
            return _friendRequestRepository.AddAsync(invitationDocument);
        }

        public async Task UpdateFriendshipAsync(Friend friend)
        {
            await _repository.UpdateAsync(friend.AsDocument());
        }
    }
}

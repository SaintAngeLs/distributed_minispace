using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Friends.Application.Dto;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
         public static Friend AsEntity(this FriendDocument document)
            => new Friend(document.UserId, document.FriendId, document.CreatedAt, document.State);

         public static FriendDocument AsDocument(this Friend entity)
            => new FriendDocument
            {
                Id = entity.Id,
                UserId = entity.UserId,
                FriendId = entity.FriendId,
                CreatedAt = entity.CreatedAt,
                State = entity.FriendState
            };

        public static FriendDto AsDto(this FriendDocument document)
            => new FriendDto
            {
                Id = document.Id,
                UserId = document.UserId,
                FriendId = document.FriendId,
                CreatedAt = document.CreatedAt,
                State = document.State,
            };

        public static FriendRequest AsEntity(this FriendRequestDocument document)
            => new FriendRequest(document.InviterId, document.InviteeId, document.RequestedAt, document.State);

        public static FriendRequestDocument AsDocument(this FriendRequest entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "FriendRequest entity cannot be null.");
            }

            if (entity.Id == null)
            {
                throw new InvalidOperationException("FriendRequest.Id must be initialized.");
            }
            return new FriendRequestDocument
            {
                Id = entity.Id,
                InviterId = entity.InviterId,
                InviteeId = entity.InviteeId,
                RequestedAt = entity.RequestedAt,
                State = entity.State
            };
        }


        public static FriendRequestDto AsDto(this FriendRequestDocument document)
            => new FriendRequestDto
            {
                Id = document.Id,
                InviterId = document.InviterId,
                InviteeId = document.InviteeId,
                RequestedAt = document.RequestedAt,
                State = document.State,
                UserId = document.InviteeId
            };

        public static UserFriendsDocument AsDocument(this UserFriends entity)
            => new UserFriendsDocument
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Friends = entity.Friends.Select(friend => friend.AsDocument()).ToList()
            };

        public static UserFriends AsEntity(this UserFriendsDocument document)
            => new UserFriends(document.UserId);  

        public static UserRequestsDocument AsDocument(this UserRequests entity)
            => new UserRequestsDocument
            {
                Id = entity.Id,
                UserId = entity.UserId,
                FriendRequests = entity.FriendRequests.Select(fr => fr.AsDocument()).ToList()
            };

        public static UserRequests AsEntity(this UserRequestsDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document), "StudentRequestsDocument cannot be null.");
            }

            var studentRequests = new UserRequests(document.UserId);
            foreach (var friendRequestDoc in document.FriendRequests)
            {
                studentRequests.AddRequest(friendRequestDoc.InviterId, friendRequestDoc.InviteeId, friendRequestDoc.RequestedAt, friendRequestDoc.State);
            }
            return studentRequests;
        }

        public static UserRequestsDto AsDto(this UserRequestsDocument document)
            => new UserRequestsDto
            {
                Id = document.Id,
                UserId = document.UserId,
                FriendRequests = document.FriendRequests.Select(fr => fr.AsDto()).ToList()
            };


    }    
}

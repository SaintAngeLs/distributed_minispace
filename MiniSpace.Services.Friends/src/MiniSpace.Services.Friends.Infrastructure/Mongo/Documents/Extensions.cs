using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Friends.Application.Dto;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
         public static Friend AsEntity(this FriendDocument document)
            => new Friend(document.StudentId, document.FriendId, document.CreatedAt, document.State);

         public static FriendDocument AsDocument(this Friend entity)
            => new FriendDocument
            {
                Id = entity.Id,
                StudentId = entity.StudentId,
                FriendId = entity.FriendId,
                CreatedAt = entity.CreatedAt,
                State = entity.FriendState
            };

        public static FriendDto AsDto(this FriendDocument document)
            => new FriendDto
            {
                Id = document.Id,
                StudentId = document.StudentId,
                FriendId = document.FriendId,
                CreatedAt = document.CreatedAt,
                State = document.State,
                // Email = document.Email,          
                // FirstName = document.FirstName,   
                // LastName = document.LastName
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
                StudentId = document.InviteeId
            };

        public static StudentFriendsDocument AsDocument(this StudentFriends entity)
            => new StudentFriendsDocument
            {
                Id = entity.Id,
                StudentId = entity.StudentId,
                Friends = entity.Friends.Select(friend => friend.AsDocument()).ToList()
            };

        public static StudentFriends AsEntity(this StudentFriendsDocument document)
            => new StudentFriends(document.StudentId);  

        // With the correct definitions of the Object-Value method in Core.
        // ...
        // public static StudentFriends AsEntity(this StudentFriendsDocument document)
        // {
        //     var studentFriends = new StudentFriends(document.StudentId);
        //     foreach (var friendDoc in document.Friends)
        //     {
        //         studentFriends.AddFriend(friendDoc.AsEntity());
        //     }
        //     return studentFriends;
        // }

         public static StudentRequestsDocument AsDocument(this StudentRequests entity)
            => new StudentRequestsDocument
            {
                Id = entity.Id,
                StudentId = entity.StudentId,
                FriendRequests = entity.FriendRequests.Select(fr => fr.AsDocument()).ToList()
            };

     public static StudentRequests AsEntity(this StudentRequestsDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document), "StudentRequestsDocument cannot be null.");
            }

            var studentRequests = new StudentRequests(document.StudentId);
            foreach (var friendRequestDoc in document.FriendRequests)
            {
                studentRequests.AddRequest(friendRequestDoc.InviterId, friendRequestDoc.InviteeId, friendRequestDoc.RequestedAt, friendRequestDoc.State);
            }
            return studentRequests;
        }

        public static StudentRequestsDto AsDto(this StudentRequestsDocument document)
            => new StudentRequestsDto
            {
                Id = document.Id,
                StudentId = document.StudentId,
                FriendRequests = document.FriendRequests.Select(fr => fr.AsDto()).ToList()
            };


    }    
}

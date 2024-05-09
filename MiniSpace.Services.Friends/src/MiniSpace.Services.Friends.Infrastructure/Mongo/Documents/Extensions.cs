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
            Console.WriteLine($"******************************************************Friend request state {entity.State}");
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
    }    
}

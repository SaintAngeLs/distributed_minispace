using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Friends.Application.Dto;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
         public static Friend AsEntity(this FriendDocument document)
            => new Friend(document.UserId, document.FriendId, document.Email, document.FirstName,
                document.LastName, document.CreatedAt, document.State);



         public static FriendDocument AsDocument(this Friend entity)
            => new FriendDocument
            {
                Id = entity.Id,
                UserId = entity.StudentId,
                FriendId = entity.FriendId,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                CreatedAt = entity.CreatedAt,
                State = entity.FriendState
            };

        public static FriendDto AsDto(this FriendDocument document)
            => new FriendDto
            {
                Id = document.Id,
                UserId = document.UserId,
                FriendId = document.FriendId,
                Email = document.Email,
                FirstName = document.FirstName,
                LastName = document.LastName,
                CreatedAt = document.CreatedAt,
                State = document.State
            };

        public static FriendRequest AsEntity(this FriendRequestDocument document)
            => new FriendRequest(document.InviterId, document.InviteeId, document.RequestedAt);

        public static FriendRequestDocument AsDocument(this FriendRequest entity)
            => new FriendRequestDocument
            {
                Id = entity.Id,
                InviterId = entity.InviterId,
                InviteeId = entity.InviteeId,
                RequestedAt = entity.RequestedAt
            };

        public static FriendRequestDto AsDto(this FriendRequestDocument document)
            => new FriendRequestDto
            {
                Id = document.Id,
                InviterId = document.InviterId,
                InviteeId = document.InviteeId,
                RequestedAt = document.RequestedAt
            };
    }    
}

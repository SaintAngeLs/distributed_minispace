using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Students.Application.Dto;
using System.Linq;
using System.Collections.Generic;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    public static class BlockedUsersExtensions
    {
        public static BlockedUsersDocument AsDocument(this BlockedUsers blockedUsers)
        {
            return new BlockedUsersDocument
            {
                Id = blockedUsers.UserId,
                UserId = blockedUsers.UserId,
                BlockedUsers = blockedUsers.BlockedUsersList.Select(b => new BlockedUsersDocument.BlockedUserEntry
                {
                    BlockedUserId = b.BlockedUserId,
                    BlockedAt = b.BlockedAt
                }).ToList()
            };
        }

        public static BlockedUsers AsEntity(this BlockedUsersDocument document)
        {
            var blockedUsers = new BlockedUsers(document.UserId);
            foreach (var entry in document.BlockedUsers)
            {
                var blockedUser = new BlockedUser(document.UserId, entry.BlockedUserId, entry.BlockedAt);
                blockedUsers.BlockUser(blockedUser.BlockedUserId); 
            }

            return blockedUsers;
        }

        public static BlockedUserDto AsDto(this BlockedUsersDocument.BlockedUserEntry blockedUserEntry, Guid blockerId)
        {
            return new BlockedUserDto
            {
                BlockerId = blockerId,
                BlockedUserId = blockedUserEntry.BlockedUserId,
                BlockedAt = blockedUserEntry.BlockedAt
            };
        }

        public static IEnumerable<BlockedUserDto> AsDto(this BlockedUsersDocument document)
        {
            return document.BlockedUsers.Select(b => b.AsDto(document.UserId));
        }
    }
}

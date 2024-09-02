using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Students.Application.Dto;
using System.Linq;

namespace MiniSpace.Services.Students.Infrastructure.Mongo
{
    public static class BlockedUsersExtensions
    {
        public static BlockedUsersDocument AsDocument(this BlockedUser blockedUser)
        {
            return new BlockedUsersDocument
            {
                Id = blockedUser.BlockerId,
                UserId = blockedUser.BlockerId,
                BlockedUsers = new[]
                {
                    new BlockedUsersDocument.BlockedUserEntry
                    {
                        BlockedUserId = blockedUser.BlockedUserId,
                        BlockedAt = blockedUser.BlockedAt
                    }
                }
            };
        }

        public static BlockedUser AsEntity(this BlockedUsersDocument document)
        {
            var blockedUserEntry = document.BlockedUsers.FirstOrDefault();

            return blockedUserEntry == null ? null : new BlockedUser(
                document.Id,
                blockedUserEntry.BlockedUserId,
                blockedUserEntry.BlockedAt);
        }

        public static BlockedUserDto AsDto(this BlockedUsersDocument.BlockedUserEntry blockedUserEntry, Guid blockerId, string blockedUserName)
        {
            return new BlockedUserDto
            {
                BlockerId = blockerId,
                BlockedUserId = blockedUserEntry.BlockedUserId,
                BlockedUserName = blockedUserName,
                BlockedAt = blockedUserEntry.BlockedAt
            };
        }
    }
}

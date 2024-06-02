using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MiniSpace.Services.Identity.Application.DTO;
using MiniSpace.Services.Identity.Core.Entities;

namespace MiniSpace.Services.Identity.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    internal static class Extensions
    {
        public static User AsEntity(this UserDocument document)
            => new User(document.Id, document.Name, document.Email, document.Password, document.Role, document.CreatedAt,
                document.Permissions);

        public static UserDocument AsDocument(this User entity)
            => new UserDocument
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Password = entity.Password,
                Role = entity.Role,
                CreatedAt = entity.CreatedAt,
                Permissions = entity.Permissions ?? Enumerable.Empty<string>()
            };

        public static UserDto AsDto(this UserDocument document)
            => new UserDto
            {
                Id = document.Id,
                Name = document.Name,
                Email = document.Email,
                Role = document.Role,
                CreatedAt = document.CreatedAt,
                Permissions = document.Permissions ?? Enumerable.Empty<string>()
            };

        public static RefreshToken AsEntity(this RefreshTokenDocument document)
            => new RefreshToken(document.Id, document.UserId, document.Token, document.CreatedAt, document.RevokedAt);
        
        public static RefreshTokenDocument AsDocument(this RefreshToken entity)
            => new RefreshTokenDocument
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Token = entity.Token,
                CreatedAt = entity.CreatedAt,
                RevokedAt = entity.RevokedAt
            };

        public static UserResetTokenDto AsDto(this UserResetToken userResetToken)
            => new UserResetTokenDto
            {
                UserId = userResetToken.UserId,
                ResetToken = userResetToken.ResetToken,
                ResetTokenExpires = userResetToken.ResetTokenExpires
            };

       public static UserResetToken AsEntity(this UserResetTokenDocument document)
            => new UserResetToken(
                document.UserId,
                document.ResetToken,
                document.ResetTokenExpires ?? DateTime.MinValue
            );

       public static UserResetTokenDocument AsDocument(this UserResetToken userResetToken)
        {
            if (userResetToken == null)
            {
                throw new ArgumentNullException(nameof(userResetToken));
            }

            return new UserResetTokenDocument
            {
                Id = userResetToken.Id,
                UserId = userResetToken.UserId,
                ResetToken = userResetToken.ResetToken,
                ResetTokenExpires = userResetToken.ResetTokenExpires
            };
        }
    }
}
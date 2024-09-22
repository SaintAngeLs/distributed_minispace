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
        {
            var user = new User(document.Id, document.Name, document.Email, document.Password, 
                Enum.Parse<Role>(document.Role, true), document.CreatedAt, document.Permissions)
            {
                IsEmailVerified = document.IsEmailVerified,
                EmailVerificationToken = document.EmailVerificationToken,
                EmailVerifiedAt = document.EmailVerifiedAt,
                IsTwoFactorEnabled = document.IsTwoFactorEnabled,
                TwoFactorSecret = document.TwoFactorSecret
            };

            // Setting the online status and IP address
            user.SetOnlineStatus(document.IsOnline, document.DeviceType);
            user.UpdateLastActive();

            return user;
        }

        // Convert User Entity to UserDocument
        public static UserDocument AsDocument(this User entity)
            => new UserDocument
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Password = entity.Password,
                Role = entity.Role.ToString(),
                CreatedAt = entity.CreatedAt,
                Permissions = entity.Permissions ?? Enumerable.Empty<string>(),
                IsEmailVerified = entity.IsEmailVerified,
                EmailVerificationToken = entity.EmailVerificationToken,
                EmailVerifiedAt = entity.EmailVerifiedAt,
                IsTwoFactorEnabled = entity.IsTwoFactorEnabled,
                TwoFactorSecret = entity.TwoFactorSecret,
                IsOnline = entity.IsOnline,                     
                DeviceType = entity.DeviceType,                 
                LastActive = entity.LastActive,

                IpAddress = entity.IpAddress
            };

        public static UserDto AsDto(this UserDocument document)
            => new UserDto
            {
                Id = document.Id,
                Name = document.Name,
                Email = document.Email,
                Role = Enum.Parse<Role>(document.Role, true),
                CreatedAt = document.CreatedAt,
                Permissions = document.Permissions ?? Enumerable.Empty<string>(),
                IsEmailVerified = document.IsEmailVerified,
                EmailVerifiedAt = document.EmailVerifiedAt,
                IsTwoFactorEnabled = document.IsTwoFactorEnabled,
                TwoFactorSecret = document.TwoFactorSecret,
                IsOnline = document.IsOnline,                     
                DeviceType = document.DeviceType,                 
                LastActive = document.LastActive,

                IpAddress = document.IpAddress
            };

        // Convert RefreshTokenDocument to RefreshToken Entity
        public static RefreshToken AsEntity(this RefreshTokenDocument document)
            => new RefreshToken(document.Id, document.UserId, document.Token, document.CreatedAt, document.RevokedAt);
        
        // Convert RefreshToken Entity to RefreshTokenDocument
        public static RefreshTokenDocument AsDocument(this RefreshToken entity)
            => new RefreshTokenDocument
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Token = entity.Token,
                CreatedAt = entity.CreatedAt,
                RevokedAt = entity.RevokedAt
            };

        // Convert UserResetToken Entity to UserResetTokenDto
        public static UserResetTokenDto AsDto(this UserResetToken userResetToken)
            => new UserResetTokenDto
            {
                UserId = userResetToken.UserId,
                ResetToken = userResetToken.ResetToken,
                ResetTokenExpires = userResetToken.ResetTokenExpires
            };

        // Convert UserResetTokenDocument to UserResetToken Entity
        public static UserResetToken AsEntity(this UserResetTokenDocument document)
            => new UserResetToken(
                document.UserId,
                document.ResetToken,
                document.ResetTokenExpires ?? DateTime.MinValue
            );

        // Convert UserResetToken Entity to UserResetTokenDocument
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

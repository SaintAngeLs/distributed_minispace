using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static Organization AsEntity(this OrganizationDocument document)
            => new Organization(
                document.Id,
                document.Name,
                document.Description,
                document.Settings,
                document.OwnerId,
                document.BannerUrl,
                document.ImageUrl,
                document.ParentOrganizationId,
                document.SubOrganizations?.Select(o => o.AsEntity())
            );

        public static OrganizationDocument AsDocument(this Organization entity)
            => new OrganizationDocument
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Settings = entity.Settings,
                BannerUrl = entity.BannerUrl,
                ImageUrl = entity.ImageUrl,
                OwnerId = entity.OwnerId,
                ParentOrganizationId = entity.ParentOrganizationId,
                SubOrganizations = entity.SubOrganizations?.Select(o => o.AsDocument()).ToList()
            };

        public static OrganizationDto AsDto(this OrganizationDocument document)
            => new OrganizationDto
            {
                Id = document.Id,
                Name = document.Name,
                Description = document.Description,
                BannerUrl = document.BannerUrl,
                ImageUrl = document.ImageUrl,
                OwnerId = document.OwnerId
            };

        public static OrganizationDetailsDto AsDetailsDto(this OrganizationDocument document)
            => new OrganizationDetailsDto
            {
                Id = document.Id,
                Name = document.Name,
                Description = document.Description,
                BannerUrl = document.BannerUrl,
                ImageUrl = document.ImageUrl,
                OwnerId = document.OwnerId,
                ParentOrganizationId = document.ParentOrganizationId,
                SubOrganizations = document.SubOrganizations?.Select(o => o.AsSubDto()).ToList(),
            };

        public static OrganizationDto AsSubDto(this OrganizationDocument document)
            => new OrganizationDto
            {
                Id = document.Id,
                Name = document.Name,
                Description = document.Description,
                BannerUrl = document.BannerUrl,
                ImageUrl = document.ImageUrl,
                OwnerId = document.OwnerId
            };

        public static Invitation AsEntity(this InvitationEntry document)
            => new Invitation(document.UserId);

        public static InvitationEntry AsDocument(this Invitation entity)
            => new InvitationEntry
            {
                UserId = entity.UserId
            };

        public static InvitationDocument AsInvitationDocument(this IEnumerable<Invitation> entities, Guid organizationId)
            => new InvitationDocument
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Invitations = entities.Select(e => e.AsDocument()).ToList()
            };

        public static IEnumerable<Invitation> AsInvitationEntities(this InvitationDocument document)
            => document.Invitations.Select(i => i.AsEntity());

        public static Role AsEntity(this RoleEntry document)
            => new Role(
                document.Name,
                document.Description,
                document.Permissions
            )
            {
                Id = document.Id
            };

        public static RoleEntry AsDocument(this Role entity)
            => new RoleEntry
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Permissions = entity.Permissions
            };

        public static RoleDocument AsRoleDocument(this IEnumerable<Role> entities, Guid organizationId)
            => new RoleDocument
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Roles = entities.Select(e => e.AsDocument()).ToList()
            };

        public static IEnumerable<Role> AsRoleEntities(this RoleDocument document)
            => document.Roles.Select(r => r.AsEntity());

        public static GalleryImage AsEntity(this GalleryImageEntry document)
            => new GalleryImage(
                document.Id,
                document.Url,
                document.OrganizationId,
                document.Description
            );

        public static GalleryImageEntry AsDocument(this GalleryImage entity)
            => new GalleryImageEntry
            {
                Id = entity.Id,
                Url = entity.Url,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt
            };

        public static GalleryImageDocument AsGalleryImageDocument(this IEnumerable<GalleryImage> entities, Guid organizationId)
            => new GalleryImageDocument
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Gallery = entities.Select(e => e.AsDocument()).ToList()
            };

        public static IEnumerable<GalleryImage> AsGalleryImageEntities(this GalleryImageDocument document)
            => document.Gallery.Select(g => g.AsEntity());

        public static User AsEntity(this UserEntry document)
            => new User(document.Id)
            {
                Roles = document.Roles?.Select(r => new Role(r.RoleName, "", new Dictionary<Permission, bool>())).ToHashSet()
            };

        public static UserEntry AsDocument(this User entity)
            => new UserEntry
            {
                Id = entity.Id,
                Roles = entity.Roles?.Select(r => new RoleAssignment
                {
                    RoleId = r.Id,
                    RoleName = r.Name
                }).ToList()
            };

        public static UserDocument AsUserDocument(this IEnumerable<User> entities, Guid organizationId)
            => new UserDocument
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Users = entities.Select(e => e.AsDocument()).ToList()
            };

        public static IEnumerable<User> AsUserEntities(this UserDocument document)
            => document.Users.Select(u => u.AsEntity());
    }
}

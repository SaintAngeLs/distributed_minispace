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
                document.DefaultRoleName,
                document.Address,       // New fields
                document.Country,
                document.City,
                document.Telephone,
                document.Email,
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
                SubOrganizations = entity.SubOrganizations?.Select(o => o.AsDocument()).ToList(),
                DefaultRoleName = entity.DefaultRoleName,
                Address = entity.Address,       // New fields
                Country = entity.Country,
                City = entity.City,
                Telephone = entity.Telephone,
                Email = entity.Email
            };

        public static OrganizationDto AsDto(this OrganizationDocument document)
            => new OrganizationDto
            {
                Id = document.Id,
                Name = document.Name,
                Description = document.Description,
                BannerUrl = document.BannerUrl,
                ImageUrl = document.ImageUrl,
                OwnerId = document.OwnerId,
                DefaultRoleName = document.DefaultRoleName
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
                SubOrganizations = document.SubOrganizations?.Select(o => new SubOrganizationDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    Description = o.Description,
                    BannerUrl = o.BannerUrl,
                    ImageUrl = o.ImageUrl,
                    OwnerId = o.OwnerId
                }).ToList(),
                DefaultRoleName = document.DefaultRoleName
            };

        public static OrganizationDto AsSubDto(this OrganizationDocument document)
            => new OrganizationDto
            {
                Id = document.Id,
                Name = document.Name,
                Description = document.Description,
                BannerUrl = document.BannerUrl,
                ImageUrl = document.ImageUrl,
                OwnerId = document.OwnerId,
                DefaultRoleName = document.DefaultRoleName
            };

        public static Invitation AsEntity(this InvitationEntry document)
            => new Invitation(document.UserId);

        public static InvitationEntry AsDocument(this Invitation entity)
            => new InvitationEntry
            {
                UserId = entity.UserId
            };

        public static OrganizationInvitationDocument AsInvitationDocument(this IEnumerable<Invitation> entities, Guid organizationId)
            => new OrganizationInvitationDocument
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Invitations = entities.Select(e => e.AsDocument()).ToList()
            };

        public static IEnumerable<Invitation> AsInvitationEntities(this OrganizationInvitationDocument document)
            => document.Invitations.Select(i => i.AsEntity());

        public static Role AsEntity(this RoleEntry document)
        {
            return new Role(
                document.Id,
                document.Name,
                document.Description,
                document.Permissions.ToDictionary(
                    kvp => Enum.Parse<Permission>(kvp.Key), 
                    kvp => kvp.Value)
            );
        }

        public static RoleEntry AsDocument(this Role entity)
        {
            return new RoleEntry
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Permissions = entity.Permissions.ToDictionary(
                    kvp => kvp.Key.ToString(), 
                    kvp => kvp.Value)
            };
        }


        public static OrganizationRolesDocument AsRoleDocument(this IEnumerable<Role> entities, Guid organizationId)
            => new OrganizationRolesDocument
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Roles = entities.Select(e => e.AsDocument()).ToList()
            };

        public static IEnumerable<Role> AsRoleEntities(this OrganizationRolesDocument document)
            => document.Roles.Select(r => r.AsEntity());


        public static OrganizationGalleryImageDocument AsGalleryImageDocument(this IEnumerable<GalleryImage> entities, Guid organizationId)
            => new OrganizationGalleryImageDocument
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Gallery = entities.Select(e => e.AsDocument()).ToList()
            };

        public static IEnumerable<GalleryImage> AsGalleryImageEntities(this OrganizationGalleryImageDocument document)
            => document.Gallery.Select(g => g.AsEntity());

       
        public static User AsEntity(this UserEntry document)
        {
            return new User(
                document.UserId, 
                new Role(
                    document.Role.RoleId, 
                    document.Role.RoleName, 
                    string.Empty, 
                    new Dictionary<Permission, bool>() 
                )
            );
        }


        public static UserEntry AsDocument(this User entity)
            => new UserEntry
            {
                UserId = entity.Id,
                Role = new RoleAssignment
                {
                    RoleId = entity.Role.Id,
                    RoleName = entity.Role.Name
                }
            };

        public static OrganizationMembersDocument AsUserDocument(this IEnumerable<User> entities, Guid organizationId)
            => new OrganizationMembersDocument
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Users = entities.Select(e => e.AsDocument()).ToList()
            };

        public static IEnumerable<User> AsUserEntities(this OrganizationMembersDocument document)
            => document.Users.Select(u => u.AsEntity());

        public static GalleryImage AsEntity(this GalleryImageEntry document)
        {
            return new GalleryImage(document.ImageId, document.ImageUrl, document.DateAdded);
        }

        public static GalleryImageEntry AsDocument(this GalleryImage entity)
        {
            return new GalleryImageEntry
            {
                ImageId = entity.ImageId,
                ImageUrl = entity.ImageUrl,
                DateAdded = entity.DateAdded
            };
        }

         public static OrganizationRequest AsEntity(this RequestDocument document)
        {
            // Assuming OrganizationRequest has a constructor or factory method that sets these properties
            return OrganizationRequest.CreateExisting(
                document.RequestId,
                document.UserId,
                document.RequestDate,
                Enum.Parse<RequestState>(document.State),
                document.Reason
            );
        }

        // Convert OrganizationRequest to RequestDocument
        public static RequestDocument AsDocument(this OrganizationRequest entity)
        {
            return new RequestDocument
            {
                RequestId = entity.Id,
                UserId = entity.UserId,
                RequestDate = entity.RequestDate,
                State = entity.State.ToString(),
                Reason = entity.Reason
            };
        }

        // Convert OrganizationRequestsDocument to OrganizationRequests
        public static OrganizationRequests AsEntity(this OrganizationRequestsDocument document)
        {
            var requests = document.Requests?.Select(r => r.AsEntity()).ToList();
            return OrganizationRequests.CreateExisting(
                document.Id,
                document.OrganizationId,
                requests
            );
        }

        // Convert OrganizationRequests to OrganizationRequestsDocument
        public static OrganizationRequestsDocument AsDocument(this OrganizationRequests entity)
        {
            return new OrganizationRequestsDocument
            {
                Id = entity.Id,
                OrganizationId = entity.OrganizationId,
                Requests = entity.Requests.Select(r => r.AsDocument()).ToList()
            };
        }
    }
}

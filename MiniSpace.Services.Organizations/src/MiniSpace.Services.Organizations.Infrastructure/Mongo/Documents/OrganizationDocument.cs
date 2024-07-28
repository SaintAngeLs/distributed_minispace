using Convey.Types;
using MiniSpace.Services.Organizations.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class OrganizationDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OrganizationSettings Settings { get; set; }
        public string BannerUrl { get; set; }
        public string ImageUrl { get; set; }
        public Guid OwnerId { get; set; }
        public IEnumerable<OrganizationDocument> SubOrganizations { get; set; }
        public IEnumerable<InvitationDocument> Invitations { get; set; }
        public IEnumerable<UserDocument> Users { get; set; }
        public IEnumerable<RoleDocument> Roles { get; set; }
        public IEnumerable<GalleryImageDocument> Gallery { get; set; }
    }
}

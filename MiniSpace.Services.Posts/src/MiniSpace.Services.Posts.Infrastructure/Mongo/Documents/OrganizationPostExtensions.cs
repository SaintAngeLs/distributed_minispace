using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Posts.Core.Entities;
using System.Collections.Generic;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public static class OrganizationPostExtensions
    {
        public static OrganizationPost AsEntity(this OrganizationPostDocument document)
            => new OrganizationPost(document.Id, document.OrganizationId, document.OrganizationPosts.Select(p => p.AsEntity()).ToList());

        public static OrganizationPostDocument AsDocument(this OrganizationPost entity)
            => new OrganizationPostDocument
            {
                Id = entity.Id,
                OrganizationId = entity.OrganizationId,
                OrganizationPosts = entity.OrganizationPosts.Select(p => p.AsDocument()).ToList()
            };
    }
}

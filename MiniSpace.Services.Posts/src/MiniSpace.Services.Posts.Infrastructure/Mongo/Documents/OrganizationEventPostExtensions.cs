using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Posts.Core.Entities;
using System.Collections.Generic;
using MongoDB.Driver;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public static class OrganizationEventPostExtensions
    {
        public static OrganizationEventPost AsEntity(this OrganizationEventPostDocument document)
            => new OrganizationEventPost(document.Id, document.OrganizationId, document.EventPosts.Select(p => p.AsEntity()).ToList());

        public static OrganizationEventPostDocument AsDocument(this OrganizationEventPost entity)
            => new OrganizationEventPostDocument
            {
                Id = entity.Id,
                OrganizationId = entity.OrganizationId,
                EventPosts = entity.EventPosts.Select(p => p.AsDocument()).ToList()
            };

        public static SortDefinition<OrganizationEventPostDocument> ToSortDefinition(IEnumerable<string> sortBy, string direction)
        {
            var builder = Builders<OrganizationEventPostDocument>.Sort;
            var sortDefinitions = new List<SortDefinition<OrganizationEventPostDocument>>();

            foreach (var sortField in sortBy)
            {
                var fieldSort = direction.ToLower() == "desc" ? builder.Descending(sortField) : builder.Ascending(sortField);
                sortDefinitions.Add(fieldSort);
            }

            return builder.Combine(sortDefinitions);
        }
    }
}

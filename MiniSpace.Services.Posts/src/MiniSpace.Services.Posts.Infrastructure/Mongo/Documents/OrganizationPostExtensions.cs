using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Posts.Core.Entities;
using System.Collections.Generic;
using MongoDB.Driver;

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
        
        public static SortDefinition<OrganizationPostDocument> ToSortDefinition(IEnumerable<string> sortBy, string direction)
        {
            var builder = Builders<OrganizationPostDocument>.Sort;
            var sortDefinitions = new List<SortDefinition<OrganizationPostDocument>>();

            foreach (var sortField in sortBy)
            {
                var fieldSort = direction.ToLower() == "desc" ? builder.Descending(sortField) : builder.Ascending(sortField);
                sortDefinitions.Add(fieldSort);
            }

            return builder.Combine(sortDefinitions);
        }
    }
}

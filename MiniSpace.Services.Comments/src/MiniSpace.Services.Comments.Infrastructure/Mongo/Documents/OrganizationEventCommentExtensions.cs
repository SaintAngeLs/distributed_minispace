using MongoDB.Driver;
using MiniSpace.Services.Comments.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Documents
{
    public static class OrganizationEventCommentExtensions
    {
        public static async Task<(IEnumerable<OrganizationEventCommentDocument> data, int totalElements, int totalPages)> AggregateByPage(
            this IMongoCollection<OrganizationEventCommentDocument> collection,
            FilterDefinition<OrganizationEventCommentDocument> filter,
            SortDefinition<OrganizationEventCommentDocument> sort,
            int pageNumber,
            int pageSize)
        {
            var totalElements = await collection.CountDocumentsAsync(filter);
            var totalPages = (int)Math.Ceiling((double)totalElements / pageSize);

            var data = await collection.Find(filter)
                .Sort(sort)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return (data, (int)totalElements, totalPages);
        }

        public static SortDefinition<OrganizationEventCommentDocument> ToSortDefinition(IEnumerable<string> sortBy, string sortDirection)
        {
            var builder = Builders<OrganizationEventCommentDocument>.Sort;
            var sort = builder.Combine(sortBy.Select(sortField =>
                sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? builder.Ascending(sortField)
                    : builder.Descending(sortField)));
            return sort;
        }
    }
}

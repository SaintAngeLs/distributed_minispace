using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Infrastructure.Mongo.Documents;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Repositories
{
    public static class Extensions
    {
        private static readonly FilterDefinitionBuilder<CommentDocument> FilterDefinitionBuilder = Builders<CommentDocument>.Filter;
        public static async Task<(int totalPages, int totalElements, IReadOnlyList<TDocument> data)> AggregateByPage<TDocument>(
            this IMongoCollection<TDocument> collection,
            FilterDefinition<TDocument> filterDefinition,
            SortDefinition<TDocument> sortDefinition,
            int page,
            int pageSize)
        {
            var countFacet = AggregateFacet.Create("count",
                PipelineDefinition<TDocument, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<TDocument>()
                }));

            var dataFacet = AggregateFacet.Create("data",
                PipelineDefinition<TDocument, TDocument>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Sort(sortDefinition),
                    PipelineStageDefinitionBuilder.Skip<TDocument>((page - 1) * pageSize),
                    PipelineStageDefinitionBuilder.Limit<TDocument>(pageSize),
                }));


            var aggregation = await collection.Aggregate()
                .Match(filterDefinition)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "count")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count;

            if (count == null)
            {
                return (0, 0, Array.Empty<TDocument>());
            }
            var totalPages = (int)Math.Ceiling((double)count / pageSize);

            var data = aggregation.First()
                .Facets.First(x => x.Name == "data")
                .Output<TDocument>();

            return (totalPages, (int)count, data);
        }
        
        public static FilterDefinition<CommentDocument> ToFilterDefinition(Guid contextId, CommentContext context)
        {
            var filterDefinition = FilterDefinitionBuilder.Empty;

            filterDefinition &= FilterDefinitionBuilder.Eq(x => x.ContextId, contextId);
            filterDefinition &= FilterDefinitionBuilder.Eq(x => x.CommentContext, context);

            return filterDefinition;
        }
        
        public static FilterDefinition<CommentDocument> AddParentFilter (this FilterDefinition<CommentDocument> filterDefinition)
        {
            filterDefinition &= FilterDefinitionBuilder.Eq(x => x.ParentId, Guid.Empty);
            return filterDefinition;
        }
        
        public static SortDefinition<CommentDocument> ToSortDefinition(IEnumerable<string> sortByArguments, string direction)
        {
            var sort = sortByArguments.ToList();
            if(!sort.Any())
            {
                sort.Add("LastReplyAt");
                sort.Add("LastUpdatedAt");
            }
            var sortDefinitionBuilder = Builders<CommentDocument>.Sort;
            var sortDefinition = sort
                .Select(sortBy => direction == "asc"
                    ? sortDefinitionBuilder.Ascending(sortBy)
                    : sortDefinitionBuilder.Descending(sortBy));
            var sortCombined = sortDefinitionBuilder.Combine(sortDefinition);
            return sortCombined;
        }
    }
}
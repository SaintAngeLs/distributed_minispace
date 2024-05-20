using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Documents;
using MongoDB.Driver;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Repositories
{
    public static class Extensions
    {
        private static readonly FilterDefinitionBuilder<PostDocument> FilterDefinitionBuilder = Builders<PostDocument>.Filter;
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
        
        public static FilterDefinition<PostDocument> ToFilterDefinition(Guid contextId, PostDocument context)
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
        
        public static FilterDefinition<CommentDocument> AddChildrenFilter (this FilterDefinition<CommentDocument> filterDefinition, 
            Guid parentId)
        {
            filterDefinition &= FilterDefinitionBuilder.Eq(x => x.ParentId, parentId);
            return filterDefinition;
        }
        
        public static SortDefinition<PostDocument> ToSortDefinition(IEnumerable<string> sortByArguments, string direction)
        {
            var sort = sortByArguments.ToList();
            if(!sort.Any())
            {
                sort.Add("LastReplyAt");
                sort.Add("LastUpdatedAt");sadadasdadaadas
            }
            var sortDefinitionBuilder = Builders<PostDocument>.Sort;
            var sortDefinition = sort
                .Select(sortBy => direction == "asc"
                    ? sortDefinitionBuilder.Ascending(sortBy)
                    : sortDefinitionBuilder.Descending(sortBy));
            var sortCombined = sortDefinitionBuilder.Combine(sortDefinition);
            return sortCombined;
        }
    }
}
using System.Collections;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Infrastructure.Mongo.Documents;
using MongoDB.Driver;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Repositories
{
    public static class Extensions
    {
        private static readonly FilterDefinitionBuilder<ReportDocument> FilterDefinitionBuilder = Builders<ReportDocument>.Filter;
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

        public static FilterDefinition<ReportDocument> ToFilterDefinition()
        {
            var filterDefinition = FilterDefinitionBuilder.Empty;

            return filterDefinition;
        }
        
        public static FilterDefinition<ReportDocument> AddContextTypesFilter (this FilterDefinition<ReportDocument> filterDefinition, 
            IEnumerable<ContextType> contextTypesEnumerable)
        {
            var contextTypes = contextTypesEnumerable.ToList();
            if(contextTypes.Any())
            {
                filterDefinition &= FilterDefinitionBuilder.In(x => x.ContextType, contextTypes);
            }
            return filterDefinition;
        }
        
        public static FilterDefinition<ReportDocument> AddStatesFilter (this FilterDefinition<ReportDocument> filterDefinition, 
            IEnumerable<ReportState> statesEnumerable)
        {
            var states = statesEnumerable.ToList();
            if(states.Count != 0)
            {
                filterDefinition &= FilterDefinitionBuilder.In(x => x.State, states);
            }
            return filterDefinition;
        }
        
        public static FilterDefinition<ReportDocument> AddReviewerIdFilter(this FilterDefinition<ReportDocument> filterDefinition, Guid reviewerId)
        {
            if (reviewerId != Guid.Empty)
            {
                filterDefinition &= FilterDefinitionBuilder.Eq(x => x.ReviewerId, reviewerId);
            }
            return filterDefinition;
        }
        
        public static FilterDefinition<ReportDocument> AddStudentIdFilter(this FilterDefinition<ReportDocument> filterDefinition, Guid studentId)
        {
            filterDefinition &= FilterDefinitionBuilder.Eq(x => x.IssuerId, studentId);
            return filterDefinition;
        }

        public static SortDefinition<ReportDocument> ToSortDefinition(IEnumerable<string> sortByArguments, string direction)
        {
            var sort = sortByArguments.ToList();
            if(sort.Count == 0)
            {
                sort.Add("UpdatedAt");
            }
            var sortDefinitionBuilder = Builders<ReportDocument>.Sort;
            var sortDefinition = sort
                .Select(sortBy => direction == "asc"
                    ? sortDefinitionBuilder.Ascending(sortBy)
                    : sortDefinitionBuilder.Descending(sortBy));
            var sortCombined = sortDefinitionBuilder.Combine(sortDefinition);
            return sortCombined;
        }
    }
}
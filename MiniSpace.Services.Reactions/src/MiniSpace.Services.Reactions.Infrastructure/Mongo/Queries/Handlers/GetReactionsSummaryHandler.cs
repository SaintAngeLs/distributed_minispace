using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Reactions.Application.Dto;
using MiniSpace.Services.Reactions.Application.Queries;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Queries.Handlers
{
    public class GetReactionsSummaryHandler : IQueryHandler<GetReactionsSummary, ReactionsSummaryDto>
    {
        private readonly IMongoRepository<ReactionDocument, Guid> _reactionRepository;

        public GetReactionsSummaryHandler(IMongoRepository<ReactionDocument, Guid> reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }
        
        public async Task<ReactionsSummaryDto>
            HandleAsync(GetReactionsSummary query, CancellationToken cancellationToken)
        {
            var documents = _reactionRepository.Collection.AsQueryable();
            documents = documents.Where(p => p.ContentId == query.ContentId && p.ContentType == query.ContentType);

            var reactions = await documents.ToListAsync();

            // Get number for each reaction type
            Dictionary<ReactionType, int> nrRcs = new();
            foreach (var r in reactions) {
                int tmp = 0;
                nrRcs.TryGetValue(r.Type, out tmp);
                nrRcs[r.Type] = tmp + 1;
            }
            if (nrRcs.Count != 0) {
                ReactionType dominant = nrRcs.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                int sum = nrRcs.Skip(1).Sum(x => x.Value);
                return new ReactionsSummaryDto(sum, dominant);
            }
            else {
                return new ReactionsSummaryDto(0, default);
            }
        }
    }    
}

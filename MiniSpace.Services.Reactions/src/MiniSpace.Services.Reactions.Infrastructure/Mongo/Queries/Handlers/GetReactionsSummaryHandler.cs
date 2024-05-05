using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Reactions.Application;
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

        private readonly IAppContext _appContext;

        public GetReactionsSummaryHandler(IMongoRepository<ReactionDocument, Guid> reactionRepository, IAppContext appContext)
        {
            _reactionRepository = reactionRepository;
            _appContext = appContext;
        }
        
        public async Task<ReactionsSummaryDto>
            HandleAsync(GetReactionsSummary query, CancellationToken cancellationToken)
        {
            var documents = _reactionRepository.Collection.AsQueryable();
            documents = documents.Where(p => p.ContentId == query.ContentId && p.ContentType == query.ContentType);

            var reactions = await documents.ToListAsync();
            var groups = reactions.GroupBy(x => x.Type);
            int nrReactions = groups.Select(x => x.ToList().Count).Sum();

            if (nrReactions == 0) {
                return new ReactionsSummaryDto(0, default, null, null);
            }

            var identity = _appContext.Identity;
            Guid? authUserReactionId = null;
            ReactionType? authUserReactionType = null;

            if (identity.IsAuthenticated && reactions.Exists(x => x.StudentId == identity.Id)) {
                var reactionDocument = reactions.Find(x => x.StudentId == identity.Id);
                authUserReactionId = reactionDocument.Id;
                authUserReactionType = reactionDocument.Type;
            }

            ReactionType dominant = groups.OrderBy(x => x.ToList().Count).Last().Key;
            return new ReactionsSummaryDto(nrReactions, dominant, authUserReactionId, authUserReactionType);
        }
    }    
}

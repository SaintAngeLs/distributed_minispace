using System.Diagnostics.CodeAnalysis;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using DnsClient;
using MiniSpace.Services.Reactions.Application.Dto;
using MiniSpace.Services.Reactions.Application.Queries;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetReactionsHandler : IQueryHandler<GetReactions, IEnumerable<ReactionDto>>
    {
        private readonly IMongoRepository<ReactionDocument, Guid> _reactionRepository;

        public GetReactionsHandler(IMongoRepository<ReactionDocument, Guid> reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }
        
        public async Task<IEnumerable<ReactionDto>> HandleAsync(GetReactions query, CancellationToken cancellationToken)
        {
            var documents = _reactionRepository.Collection.AsQueryable();
            documents = documents.Where(p => p.ContentId == query.ContentId && p.ContentType == query.ContentType);

            var reactions = await documents.ToListAsync();
            return reactions.Select(p => p.AsDto());
        }
    }    
}

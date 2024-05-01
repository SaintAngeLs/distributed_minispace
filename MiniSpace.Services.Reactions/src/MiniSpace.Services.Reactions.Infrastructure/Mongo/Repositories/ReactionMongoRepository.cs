using Convey.Persistence.MongoDB;
using Jaeger.Propagation;
using Microsoft.AspNetCore.Components.Forms;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Repositories
{
    public class ReactionMongoRepository(IMongoRepository<ReactionDocument, Guid> repository) : IReactionRepository
    {
        private readonly IMongoRepository<ReactionDocument, Guid> _repository = repository;

        public async Task<(int NumberOfReactions, ReactionType DominantReaction)> GetReactionSummaryAsync(Guid contentId, ReactionContentType contentType)
        {
            // Get number for each reaction type
            Dictionary<ReactionType, int> nrRcs = new();
            var reactions = _repository.Collection.AsQueryable();
            var reactionsAsync = await reactions.ToListAsync();
            foreach (var r in reactionsAsync) {
                int tmp = 0;
                nrRcs.TryGetValue(r.Type, out tmp);
                nrRcs[r.Type] = tmp + 1;
            }
            ReactionType dominant = nrRcs.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            int sum = nrRcs.Skip(1).Sum(x => x.Value);
            return (sum, dominant);
        }

        public async Task<IEnumerable<Reaction>> GetReactionsAsync(Guid contentId, ReactionContentType contentType)
        {
            var reactions = _repository.Collection.AsQueryable();
            var reactionsAsync = await reactions.ToListAsync();
            return reactionsAsync.Select(x => x.AsEntity());
        }

        public Task AddAsync(Reaction reaction)
        {
            return _repository.AddAsync(reaction.AsDocument());
        }

        public Task DeleteAsync(Guid studentId, Guid contentId, ReactionContentType contentType)
        {
            return _repository.DeleteAsync(x => x.StudentId == studentId && x.ContentId == contentId);
        }

        public async Task<Reaction> GetAsync(Guid studentId, Guid contentId, ReactionContentType contentType)
        {
            var reaction = await _repository.GetAsync(x => x.StudentId == studentId && x.ContentId == contentId && x.ContentType ==
                        contentType);
            return reaction?.AsEntity();
        }
    }    
}

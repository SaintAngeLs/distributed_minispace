using System.Diagnostics.CodeAnalysis;
using Paralax.Persistence.MongoDB;
using Jaeger.Propagation;
using Microsoft.AspNetCore.Components.Forms;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
    public class ReactionMongoRepository(IMongoRepository<ReactionDocument, Guid> repository) : IReactionRepository
    {
        private readonly IMongoRepository<ReactionDocument, Guid> _repository = repository;

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

        public async Task<Reaction> GetAsync(Guid id)
        {
            var reaction = await _repository.GetAsync(x => x.Id == id);
            return reaction?.AsEntity();
        }

        public Task DeleteAsync(Guid id)
        {
            return _repository.DeleteAsync(x => x.Id == id);
        }
        
        public Task<bool> ExistsAsync(Guid contentId, ReactionContentType contentType, Guid studentId)
            => _repository.ExistsAsync(x => x.ContentId == contentId && x.ContentType == contentType 
                                                                     && x.UserId == studentId);

        public async Task<Reaction> GetByIdAsync(Guid id, Guid userId)
        {
            var reaction = await _repository.GetAsync(x => x.Id == id && x.UserId == userId);
            return reaction?.AsEntity();
        }
    }    
}

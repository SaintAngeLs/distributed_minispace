using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Extensions;
using MongoDB.Driver;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Repositories
{
    public class ReactionsOrganizationsEventCommentsMongoRepository : IReactionsOrganizationsEventCommentsRepository
    {
        private readonly IMongoRepository<OrganizationEventCommentsReactionDocument, Guid> _repository;

        public ReactionsOrganizationsEventCommentsMongoRepository(IMongoRepository<OrganizationEventCommentsReactionDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(x => x.OrganizationEventCommentId == id);

        public async Task<Reaction> GetByIdAsync(Guid id)
        {
            var document = await _repository.GetAsync(x => x.Reactions.Any(r => r.Id == id));
            return document?.Reactions.FirstOrDefault(r => r.Id == id)?.AsEntity();
        }

        public async Task AddAsync(Reaction reaction)
        {
            var filter = Builders<OrganizationEventCommentsReactionDocument>.Filter.Eq(d => d.OrganizationEventCommentId, reaction.ContentId);

            var update = Builders<OrganizationEventCommentsReactionDocument>.Update.Combine(
                Builders<OrganizationEventCommentsReactionDocument>.Update.Push(d => d.Reactions, reaction.AsDocument()),
                Builders<OrganizationEventCommentsReactionDocument>.Update.SetOnInsert(d => d.OrganizationEventCommentId, reaction.ContentId),
                Builders<OrganizationEventCommentsReactionDocument>.Update.SetOnInsert(d => d.Id, Guid.NewGuid())
            );

            var options = new UpdateOptions { IsUpsert = true };
            var result = await _repository.Collection.UpdateOneAsync(filter, update, options);

            if (!result.IsAcknowledged || result.ModifiedCount == 0)
            {
            }
        }
        
        public async Task UpdateAsync(Reaction reaction)
        {
            var filter = Builders<OrganizationEventCommentsReactionDocument>.Filter.And(
                Builders<OrganizationEventCommentsReactionDocument>.Filter.Eq(x => x.OrganizationEventCommentId, reaction.ContentId),
                Builders<OrganizationEventCommentsReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.Id == reaction.Id)
            );

            var update = Builders<OrganizationEventCommentsReactionDocument>.Update.Set(x => x.Reactions[-1], reaction.AsDocument());

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<OrganizationEventCommentsReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.Id == id);
            var update = Builders<OrganizationEventCommentsReactionDocument>.Update.PullFilter(x => x.Reactions, r => r.Id == id);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Reaction>> GetByContentIdAsync(Guid contentId)
        {
            var document = await _repository.GetAsync(d => d.OrganizationEventCommentId == contentId);
            return document?.Reactions.Select(r => r.AsEntity()) ?? Enumerable.Empty<Reaction>();
        }

        public async Task<Reaction> GetAsync(Guid contentId, Guid userId)
        {
            var filter = Builders<OrganizationEventCommentsReactionDocument>.Filter.And(
                Builders<OrganizationEventCommentsReactionDocument>.Filter.Eq(x => x.OrganizationEventCommentId, contentId),
                Builders<OrganizationEventCommentsReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.UserId == userId)
            );

            var document = await _repository.Collection.Find(filter).FirstOrDefaultAsync();
            return document?.Reactions.FirstOrDefault(r => r.UserId == userId)?.AsEntity();
        }

    }
}

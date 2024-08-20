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
            var filter = Builders<OrganizationEventCommentsReactionDocument>.Filter.Eq(x => x.OrganizationEventCommentId, reaction.ContentId);
            var update = Builders<OrganizationEventCommentsReactionDocument>.Update.Push(x => x.Reactions, reaction.AsDocument());

            await _repository.Collection.UpdateOneAsync(filter, update);
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
    }
}

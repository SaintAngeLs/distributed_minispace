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
    public class ReactionsOrganizationsPostCommentsMongoRepository : IReactionsOrganizationsPostCommentsRepository
    {
        private readonly IMongoRepository<OrganizationPostCommentsReactionDocument, Guid> _repository;

        public ReactionsOrganizationsPostCommentsMongoRepository(IMongoRepository<OrganizationPostCommentsReactionDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(x => x.OrganizationPostCommentId == id);

        public async Task<Reaction> GetByIdAsync(Guid id)
        {
            var document = await _repository.GetAsync(x => x.Reactions.Any(r => r.Id == id));
            return document?.Reactions.FirstOrDefault(r => r.Id == id)?.AsEntity();
        }

        public async Task AddAsync(Reaction reaction)
        {
            var filter = Builders<OrganizationPostCommentsReactionDocument>.Filter.Eq(x => x.OrganizationPostCommentId, reaction.ContentId);

            var update = Builders<OrganizationPostCommentsReactionDocument>.Update.Combine(
                Builders<OrganizationPostCommentsReactionDocument>.Update.Push(x => x.Reactions, reaction.AsDocument()),
                Builders<OrganizationPostCommentsReactionDocument>.Update.SetOnInsert(x => x.OrganizationPostCommentId, reaction.ContentId)
            );

            var result = await _repository.Collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            if (!result.IsAcknowledged || result.ModifiedCount == 0)
            {
            }
        }


        public async Task UpdateAsync(Reaction reaction)
        {
            var filter = Builders<OrganizationPostCommentsReactionDocument>.Filter.And(
                Builders<OrganizationPostCommentsReactionDocument>.Filter.Eq(x => x.OrganizationPostCommentId, reaction.ContentId),
                Builders<OrganizationPostCommentsReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.Id == reaction.Id)
            );

            var update = Builders<OrganizationPostCommentsReactionDocument>.Update.Set(x => x.Reactions[-1], reaction.AsDocument());

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<OrganizationPostCommentsReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.Id == id);
            var update = Builders<OrganizationPostCommentsReactionDocument>.Update.PullFilter(x => x.Reactions, r => r.Id == id);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }
    }
}

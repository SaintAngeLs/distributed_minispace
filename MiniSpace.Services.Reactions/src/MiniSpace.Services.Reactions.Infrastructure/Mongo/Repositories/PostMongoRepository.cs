using Convey.Persistence.MongoDB;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Repositories
{
    public class PostMongoRepository : IPostRepository
    {
        private readonly IMongoRepository<PostDocument, Guid> _repository;

        public PostMongoRepository(IMongoRepository<PostDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task AddAsync(Post post)
        {
            return _repository.AddAsync(post.AsDocument());
        }

        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(s => s.Id == id);
    }    
}

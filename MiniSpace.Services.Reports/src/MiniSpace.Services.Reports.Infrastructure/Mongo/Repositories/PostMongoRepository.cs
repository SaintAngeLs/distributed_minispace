using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Core.Repositories;
using MiniSpace.Services.Reports.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Repositories
{
    public class PostMongoRepository : IPostRepository
    {
        private readonly IMongoRepository<PostDocument, Guid> _repository;

        public PostMongoRepository(IMongoRepository<PostDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<Post> GetAsync(Guid id)
        {
            var post = await _repository.GetAsync(p => p.Id == id);

            return post?.AsEntity();
        }

        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(p => p.Id == id);

        public Task AddAsync(Post post)
            => _repository.AddAsync(post.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }    
}
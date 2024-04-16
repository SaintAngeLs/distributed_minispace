using Convey.Persistence.MongoDB;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Repositories
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

        public Task AddAsync(Post post)
            => _repository.AddAsync(post.AsDocument());

        public Task UpdateAsync(Post post)
            => _repository.UpdateAsync(post.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }    
}

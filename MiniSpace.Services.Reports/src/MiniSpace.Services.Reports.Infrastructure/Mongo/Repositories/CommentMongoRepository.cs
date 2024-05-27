using Convey.Persistence.MongoDB;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Core.Repositories;
using MiniSpace.Services.Reports.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Repositories
{
    public class CommentMongoRepository : ICommentRepository
    {
        private readonly IMongoRepository<CommentDocument, Guid> _repository;

        public CommentMongoRepository(IMongoRepository<CommentDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<Comment> GetAsync(Guid id)
        {
            var comment = await _repository.GetAsync(c => c.Id == id);

            return comment?.AsEntity();
        }

        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(c => c.Id == id);

        public Task AddAsync(Comment comment)
            => _repository.AddAsync(comment.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }    
}
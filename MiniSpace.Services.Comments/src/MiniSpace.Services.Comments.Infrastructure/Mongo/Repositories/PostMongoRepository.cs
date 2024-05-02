using Convey.Persistence.MongoDB;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Repositories
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
            var Comment = await _repository.GetAsync(p => p.Id == id);

            return Comment?.AsEntity();
        }
        
        public Task AddAsync(Comment Comment)
            => _repository.AddAsync(Comment.AsDocument());

        public Task UpdateAsync(Comment Comment)
            => _repository.UpdateAsync(Comment.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }    
}

using Convey.Persistence.MongoDB;
using Microsoft.Extensions.Hosting;
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
            var comment = await _repository.GetAsync(p => p.Id == id);

            return comment?.AsEntity();
        }
        
        public Task AddAsync(Comment comment)
            => _repository.AddAsync(comment.AsDocument());

        public Task UpdateAsync(Comment comment)
            => _repository.UpdateAsync(comment.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);

        public async Task<IEnumerable<Comment>> GetByEventIdAsync(Guid eventId)
        {
            var comments = _repository.Collection.AsQueryable();
            var commentsByEventId = await comments.Where(e =>e.CommentContext == CommentContext.Event && e.ContextId == eventId).ToListAsync();
            return commentsByEventId.Select(e => e.AsEntity());
        }

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId)
        {
            var comments = _repository.Collection.AsQueryable();
            var commentsByEventId = await comments.Where(e => e.CommentContext == CommentContext.Post && e.ContextId == postId).ToListAsync();
            return commentsByEventId.Select(e => e.AsEntity());
        }
    }    
}

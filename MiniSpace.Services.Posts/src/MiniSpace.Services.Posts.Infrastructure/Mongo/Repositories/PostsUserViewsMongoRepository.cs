using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Repositories
{
    public class PostsUserViewsMongoRepository : IPostsUserViewsRepository
    {
        private readonly IMongoRepository<UserPostsViewsDocument, Guid> _repository;

        public PostsUserViewsMongoRepository(IMongoRepository<UserPostsViewsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PostsViews> GetAsync(Guid userId)
        {
            var document = await _repository.GetAsync(x => x.UserId == userId);
            return document?.ToEntity();
        }

        public async Task AddAsync(PostsViews postsViews)
        {
            var document = postsViews.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task UpdateAsync(PostsViews postsViews)
        {
            var document = postsViews.AsDocument();
            await _repository.UpdateAsync(document);
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _repository.DeleteAsync(x => x.UserId == userId);
        }
    }
}

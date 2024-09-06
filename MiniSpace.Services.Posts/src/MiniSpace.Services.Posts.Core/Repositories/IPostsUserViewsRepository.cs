using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IPostsUserViewsRepository
    {
        Task<PostsViews> GetAsync(Guid userId);
        Task AddAsync(PostsViews postsViews);
        Task UpdateAsync(PostsViews postsViews);
        Task DeleteAsync(Guid userId);
    }
}

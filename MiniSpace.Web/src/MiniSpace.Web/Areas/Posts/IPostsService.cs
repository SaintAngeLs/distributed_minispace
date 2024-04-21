using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;

namespace MiniSpace.Web.Areas.Posts
{
    public interface IPostsService
    {
        Task ChangePostStateAsync(Guid postId, string state, DateTime publishDate); 
        Task CreatePostAsync(Guid postId, Guid eventId, Guid studentId, string textContext, string mediaContext,
            string state, DateTime publishedDate);
        Task DeletePostAsync(Guid postId);
        Task<IEnumerable<PostDto>> GetPostsAsync(Guid eventId);
        Task UpdatePostAsync(Guid postId, string textContext, string mediaContext);
    }
}
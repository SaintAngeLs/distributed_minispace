using MiniSpace.Services.Notifications.Application.Dto.Posts;

namespace MiniSpace.Services.Notifications.Application.Services.Clients
{
    public interface IPostsServiceClient
    {
        Task<PostDto> GetPostAsync(Guid postId);
    }
}

using System;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Services.Clients
{
    public interface ICommentsServiceClient
    {
        Task<CommentDto> GetCommentAsync(Guid commentId);
    }
}
using System;
using System.Threading.Tasks;

namespace MiniSpace.Services.Reactions.Application.Services.Clients
{
    public interface ICommentServiceClient
    {
        Task<bool> CommentExistsAsync(Guid id);
    }
}

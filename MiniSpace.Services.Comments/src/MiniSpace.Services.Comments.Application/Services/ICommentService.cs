using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Comments.Application.Commands;
using MiniSpace.Services.Comments.Application.Dto;
using MiniSpace.Services.Comments.Application.Wrappers;
using MiniSpace.Services.Comments.Core.Wrappers;

namespace MiniSpace.Services.Comments.Application.Services
{
    public interface ICommentService
    {
        Task<PagedResponse<CommentDto>> BrowseCommentsAsync(SearchComments command);
    }
}
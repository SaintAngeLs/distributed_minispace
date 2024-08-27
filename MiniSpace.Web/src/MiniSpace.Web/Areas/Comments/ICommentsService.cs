using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Comments.CommandDto;
using MiniSpace.Web.Areas.Comments.CommandsDto;
using MiniSpace.Web.DTO.Comments;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Comments
{
    public interface ICommentsService
    {
        Task<PagedResponseDto<CommentDto>> SearchRootCommentsAsync(SearchRootCommentsCommand command);
        Task<HttpResponse<PagedResponseDto<CommentDto>>> SearchSubCommentsAsync(SearchSubCommentsCommand command);
        Task<CommentDto> GetCommentAsync(Guid commentId);
        Task<HttpResponse<object>> CreateCommentAsync(CreateCommentCommand command);
        Task<HttpResponse<object>> UpdateCommentAsync(UpdateCommentCommand command);
        Task DeleteCommentAsync(Guid commentId);
        Task<HttpResponse<object>> AddLikeAsync(AddLikeDto command);
        Task DeleteLikeAsync(Guid commentId);
    }
}

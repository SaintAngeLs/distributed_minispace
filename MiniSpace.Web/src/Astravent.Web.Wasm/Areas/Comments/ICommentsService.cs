using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.Areas.Comments.CommandsDto;
using Astravent.Web.Wasm.DTO.Comments;
using Astravent.Web.Wasm.DTO.Wrappers;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Comments
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

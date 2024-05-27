using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Enums;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Comments
{
    public interface ICommentsService
    {
        Task<HttpResponse<PagedResponseDto<IEnumerable<CommentDto>>>> SearchRootCommentsAsync(Guid contextId,
            string commentContext, PageableDto pageable);
        Task<HttpResponse<PagedResponseDto<IEnumerable<CommentDto>>>> SearchSubCommentsAsync(Guid contextId,
            string commentContext, Guid parentId, PageableDto pageable);
        Task<HttpResponse<object>> CreateCommentAsync(Guid commentId, Guid contextId, string commentContext,
            Guid studentId, Guid parentId, string comment);
        Task<HttpResponse<object>> UpdateCommentAsync(Guid commentId, string textContext);
        Task DeleteCommentAsync(Guid commentId);
        Task AddLikeAsync(Guid commentId);
        Task DeleteLikeAsync(Guid commentId);
    }    
}

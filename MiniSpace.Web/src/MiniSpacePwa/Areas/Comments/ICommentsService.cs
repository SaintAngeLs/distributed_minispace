using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpacePwa.DTO;
using MiniSpacePwa.DTO.Enums;
using MiniSpacePwa.DTO.Wrappers;
using MiniSpacePwa.HttpClients;

namespace MiniSpacePwa.Areas.Comments
{
    public interface ICommentsService
    {
        Task<HttpResponse<PagedResponseDto<IEnumerable<CommentDto>>>> SearchRootCommentsAsync(Guid contextId,
            string commentContext, PageableDto pageable);
        Task<HttpResponse<PagedResponseDto<IEnumerable<CommentDto>>>> SearchSubCommentsAsync(Guid contextId,
            string commentContext, Guid parentId, PageableDto pageable);
        Task<CommentDto> GetCommentAsync(Guid commentId);
        Task<HttpResponse<object>> CreateCommentAsync(Guid commentId, Guid contextId, string commentContext,
            Guid studentId, Guid parentId, string comment);
        Task<HttpResponse<object>> UpdateCommentAsync(Guid commentId, string textContext);
        Task DeleteCommentAsync(Guid commentId);
        Task AddLikeAsync(Guid commentId);
        Task DeleteLikeAsync(Guid commentId);
    }    
}

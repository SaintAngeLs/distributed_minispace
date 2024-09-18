using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astravent.Web.Wasm.DTO.Comments
{
    public static class CommentExtensions
    {
        public static ReplyDto ToReplyDto(this CommentDto comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            return new ReplyDto
            {
                Id = comment.Id,
                ParentId = comment.ParentId,
                UserId = comment.UserId,
                TextContent = comment.TextContent,
                CreatedAt = comment.CreatedAt,
                IsDeleted = comment.IsDeleted
            };
        }
    }
}
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

        public static CommentDto ToCommentDto(this ReplyDto reply)
        {
            if (reply == null)
                throw new ArgumentNullException(nameof(reply));

            return new CommentDto
            {
                Id = reply.Id,
                ParentId = reply.ParentId,
                UserId = reply.UserId,
                TextContent = reply.TextContent,
                CreatedAt = reply.CreatedAt,
                IsDeleted = reply.IsDeleted,
                Replies = new List<ReplyDto>() 
            };
        }
    }
}
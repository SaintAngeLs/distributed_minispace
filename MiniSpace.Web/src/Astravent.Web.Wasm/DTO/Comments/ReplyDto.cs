using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astravent.Web.Wasm.DTO.Comments
{
    public class ReplyDto
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid UserId { get; set; }
        public string TextContent { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public ReplyDto()
        {
        }

        public ReplyDto(ReplyDto reply)
        {
            Id = reply.Id;
            ParentId = reply.ParentId;
            UserId = reply.UserId;
            TextContent = reply.TextContent;
            CreatedAt = reply.CreatedAt;
            IsDeleted = reply.IsDeleted;
        }
    }
}
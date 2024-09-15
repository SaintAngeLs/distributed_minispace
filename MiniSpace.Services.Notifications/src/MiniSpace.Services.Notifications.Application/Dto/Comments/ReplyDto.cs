using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Application.Dto.Comments
{
    public class ReplyDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
        public string TextContent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
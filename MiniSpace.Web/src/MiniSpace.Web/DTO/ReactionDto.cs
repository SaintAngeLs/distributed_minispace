using System;
using MiniSpace.Web.DTO.Enums;

namespace MiniSpace.Web.DTO
{
    public class ReactionDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentFullName { get; set; }
        public Guid ContentId { get; set; }
        public ReactionContentType ContentType { get; set; }
        public ReactionType Type { get; set; }
    }    
}

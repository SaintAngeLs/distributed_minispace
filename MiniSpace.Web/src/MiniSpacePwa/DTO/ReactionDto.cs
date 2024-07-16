using System;
using MiniSpacePwa.DTO.Enums;

namespace MiniSpacePwa.DTO
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

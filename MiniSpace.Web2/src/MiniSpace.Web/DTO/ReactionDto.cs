using System;
using MiniSpace.Web.DTO.Enums;
using MiniSpace.Web.DTO.Enums.Reactions;

namespace MiniSpace.Web.DTO
{
    public class ReactionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ContentId { get; set; }
        public ReactionContentType ContentType { get; set; }
        public ReactionType ReactionType { get; set; }
        public ReactionTargetType TargetType { get; set; }
    }    
}

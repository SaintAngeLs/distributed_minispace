using System;
using Astravent.Web.Wasm.DTO.Enums;
using Astravent.Web.Wasm.DTO.Enums.Reactions;

namespace Astravent.Web.Wasm.DTO
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astravent.Web.Wasm.Areas.Reactions.CommandDto
{
    public class CreateReactionDto
    {
        public Guid ReactionId { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string ReactionType { get; set; }
        // ReactionType :=  LoveIt || LikeIt || Wow || ItWasOkay || HateIt
        public Guid ContentId { get; set; }
        public string ContentType { get; set; }
        // ContentType :=  Post || Event || Comment
        public string TargetType { get; set; }
        // ReactionTargetType :=  User || Organization
    }
}
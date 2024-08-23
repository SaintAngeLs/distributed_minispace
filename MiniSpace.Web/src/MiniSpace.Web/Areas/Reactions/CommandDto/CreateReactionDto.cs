using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Web.Areas.Reactions.CommandDto
{
    public class CreateReactionDto
    {
        public Guid ReactionId { get; set;}
        public Guid UserId { get; }
        public string ReactionType { get; }
        // ReactionType :=  LoveIt || LikeIt || Wow || ItWasOkay || HateIt
        public Guid ContentId { get; }
        public string ContentType { get; }
        // ContentType :=  Post || Event || Comment
        public string TargetType { get; }
        // ReactionTargetType :=  User || Organization
    }
}
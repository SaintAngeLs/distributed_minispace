using System;

namespace MiniSpace.Web.Areas.Reactions.CommandDto
{
    public class UpdateReactionDto
    {
        public Guid ReactionId { get; set; }
        public Guid UserId { get; set; }
        public string NewReactionType { get; set; }
    }
}

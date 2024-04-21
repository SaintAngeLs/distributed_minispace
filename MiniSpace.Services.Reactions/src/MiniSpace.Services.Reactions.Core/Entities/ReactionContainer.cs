
using System.ComponentModel;

namespace MiniSpace.Services.Reactions.Core.Entities
{
    public class ReactionContainer
    {
        public Guid ContentId;
        public ReactionContentType ContentType;
        IEnumerable<Reaction> Reactions;

        public ReactionContainer(Guid contentId, ReactionContentType contentType, IEnumerable<Reaction> reactions)
        {
            ContentId = contentId;
            ContentType = contentType;
            Reactions = reactions;
        }
    }    
}

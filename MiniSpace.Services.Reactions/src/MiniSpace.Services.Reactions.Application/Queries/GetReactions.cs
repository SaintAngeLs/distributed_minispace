
using Convey.CQRS.Queries;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Queries
{
    public class GetReactions : IQuery<IEnumerable<Reaction>>
    {
        public Guid ContentId { get; set; }
        public ReactionContentType ContentType { get; set; }
    }
}
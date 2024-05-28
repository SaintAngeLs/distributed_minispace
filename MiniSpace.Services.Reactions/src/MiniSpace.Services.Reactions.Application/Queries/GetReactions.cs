
using System.Diagnostics.CodeAnalysis;
using Convey.CQRS.Queries;
using MiniSpace.Services.Reactions.Application.Dto;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetReactions : IQuery<IEnumerable<ReactionDto>>
    {
        public Guid ContentId { get; set; }
        public ReactionContentType ContentType { get; set; }
    }
}
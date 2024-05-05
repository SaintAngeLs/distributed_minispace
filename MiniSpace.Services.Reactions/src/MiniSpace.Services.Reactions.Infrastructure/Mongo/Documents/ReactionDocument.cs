using Convey.Types;
using Convey.WebApi.CQRS;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents
{
    public class ReactionDocument : IIdentifiable<Guid>
    {
        public Guid Id {get;set;}
        public Guid StudentId {get;set;}
        public Guid ContentId{get;set;}
        public ReactionContentType ContentType{get;set;}
        public ReactionType Type {get;set;}
    }
}

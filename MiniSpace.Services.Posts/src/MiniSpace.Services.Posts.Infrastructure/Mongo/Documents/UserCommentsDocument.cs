using System;
using System.Collections.Generic;
using Convey.Types;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    public class UserCommentsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; } 
        public Guid UserId { get; set; } 
        public IEnumerable<CommentDocument> Comments { get; set; } 
    }
}

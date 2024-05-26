using Convey.Types;
using MiniSpace.Services.Friends.Core.Entities;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Documents
{
    public class StudentFriendsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; } 
        public Guid StudentId { get; set; }
        public List<FriendDocument> Friends { get; set; } = new List<FriendDocument>(); // List of friend documents
    }
}

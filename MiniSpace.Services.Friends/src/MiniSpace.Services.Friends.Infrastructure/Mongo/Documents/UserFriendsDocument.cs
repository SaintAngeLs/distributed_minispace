using Convey.Types;
using MiniSpace.Services.Friends.Core.Entities;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Documents
{
    public class UserFriendsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; } 
        public Guid UserId { get; set; }
        public List<FriendDocument> Friends { get; set; } = new List<FriendDocument>(); 
    }
}

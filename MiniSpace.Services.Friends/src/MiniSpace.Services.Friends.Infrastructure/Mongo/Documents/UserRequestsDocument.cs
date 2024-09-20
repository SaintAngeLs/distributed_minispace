using Convey.Types;
using MiniSpace.Services.Friends.Core.Entities;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Documents
{
    public class UserRequestsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<FriendRequestDocument> FriendRequests { get; set; } = new List<FriendRequestDocument>();
    }
}

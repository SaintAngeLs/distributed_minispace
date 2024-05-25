using Convey.Types;
using MiniSpace.Services.Friends.Core.Entities;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Documents
{
    public class StudentRequestsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public List<FriendRequestDocument> FriendRequests { get; set; } = new List<FriendRequestDocument>();
    }
}

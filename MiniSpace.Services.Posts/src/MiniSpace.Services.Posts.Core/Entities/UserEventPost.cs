using System;
using System.Collections.Generic;
using MiniSpace.Services.Posts.Core.Events;

namespace MiniSpace.Services.Posts.Core.Entities
{
    public class UserEventPost : AggregateRoot
    {
        public Guid UserId { get; private set; }
        public List<Post> UserEventPosts { get; private set; }

        public UserEventPost(Guid id, Guid userId, List<Post> userEventPosts)
        {
            Id = id;
            UserId = userId;
            UserEventPosts = userEventPosts;
        }

        public void AddUserEventPost(Post post)
        {
            UserEventPosts.Add(post);
            AddEvent(new UserEventPostAddedEvent(Id, post.Id));
        }

        public void RemoveUserEventPost(Guid postId)
        {
            var post = UserEventPosts.FirstOrDefault(p => p.Id == postId);
            if (post != null)
            {
                UserEventPosts.Remove(post);
                AddEvent(new UserEventPostRemovedEvent(Id, postId));
            }
        }
    }
}

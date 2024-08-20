using System;
using System.Collections.Generic;
using MiniSpace.Services.Posts.Core.Events;

namespace MiniSpace.Services.Posts.Core.Entities
{
    public class UserPost : AggregateRoot
    {
        public Guid UserId { get; private set; }
        public List<Post> UserPosts { get; private set; }

        public UserPost(Guid id, Guid userId, List<Post> userPosts)
        {
            Id = id;
            UserId = userId;
            UserPosts = userPosts;
        }

        public void AddUserPost(Post post)
        {
            UserPosts.Add(post);
            AddEvent(new UserPostAddedEvent(Id, post.Id));
        }

        public void RemoveUserPost(Guid postId)
        {
            var post = UserPosts.FirstOrDefault(p => p.Id == postId);
            if (post != null)
            {
                UserPosts.Remove(post);
                AddEvent(new UserPostRemovedEvent(Id, postId));
            }
        }
    }
}

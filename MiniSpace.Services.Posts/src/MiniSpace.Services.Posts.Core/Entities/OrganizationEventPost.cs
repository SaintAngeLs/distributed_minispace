using System;
using System.Collections.Generic;
using MiniSpace.Services.Posts.Core.Events;

namespace MiniSpace.Services.Posts.Core.Entities
{
    public class OrganizationEventPost : AggregateRoot
    {
        public Guid OrganizationId { get; private set; }
        public List<Post> EventPosts { get; private set; }

        public OrganizationEventPost(Guid id, Guid organizationId, List<Post> eventPosts)
        {
            Id = id;
            OrganizationId = organizationId;
            EventPosts = eventPosts;
        }

        public void AddEventPost(Post post)
        {
            EventPosts.Add(post);
            AddEvent(new OrganizationEventPostAddedEvent(Id, post.Id));
        }

        public void RemoveEventPost(Guid postId)
        {
            var post = EventPosts.FirstOrDefault(p => p.Id == postId);
            if (post != null)
            {
                EventPosts.Remove(post);
                AddEvent(new OrganizationEventPostRemovedEvent(Id, postId));
            }
        }
    }
}

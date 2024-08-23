using System;
using System.Collections.Generic;
using MiniSpace.Services.Posts.Core.Events;

namespace MiniSpace.Services.Posts.Core.Entities
{
    public class OrganizationPost : AggregateRoot
    {
        public Guid OrganizationId { get; private set; }
        public List<Post> OrganizationPosts { get; private set; }

        public OrganizationPost(Guid id, Guid organizationId, List<Post> organizationPosts)
        {
            Id = id;
            OrganizationId = organizationId;
            OrganizationPosts = organizationPosts;
        }

        public void AddOrganizationPost(Post post)
        {
            OrganizationPosts.Add(post);
            AddEvent(new OrganizationEventPostAddedEvent(Id, post.Id));
        }

        public void RemoveOrganizationPost(Guid postId)
        {
            var post = OrganizationPosts.FirstOrDefault(p => p.Id == postId);
            if (post != null)
            {
                OrganizationPosts.Remove(post);
                AddEvent(new OrganizationPostRemovedEvent(Id, postId));
            }
        }
    }
}

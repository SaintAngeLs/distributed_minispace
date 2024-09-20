using System;
using System.Collections.Generic;
using System.Linq;
using Convey.Types;
using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    public class UserPostsViewsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<ViewDocument> Views { get; set; } = new List<ViewDocument>();

        public static UserPostsViewsDocument FromEntity(PostsViews eventsViews)
        {
            return new UserPostsViewsDocument
            {
                Id = Guid.NewGuid(), 
                UserId = eventsViews.UserId,
                Views = new List<ViewDocument>(eventsViews.Views.Select(ViewDocument.FromEntity))
            };
        }

        public PostsViews ToEntity()
        {
            return new PostsViews(UserId, Views.Select(view => view.ToEntity()));
        }
    }
}

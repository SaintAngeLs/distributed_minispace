using System;
using System.Linq;
using MiniSpace.Services.Posts.Application.DTO;
using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    public static class PostsViewsExtensions
    {
        public static UserPostsViewsDto AsDto(this UserPostsViewsDocument document)
        {
            return new UserPostsViewsDto
            {
                UserId = document.UserId,
                Views = document.Views.Select(v => new ViewDto
                {
                    PostId = v.PostId,
                    Date = v.Date
                })
            };
        }

        public static UserPostsViewsDocument AsDocument(this PostsViews entity)
        {
            return new UserPostsViewsDocument
            {
                Id = Guid.NewGuid(),
                UserId = entity.UserId,
                Views = entity.Views.Select(ViewDocument.FromEntity).ToList()
            };
        }

        public static PostsViews AsEntity(this UserPostsViewsDocument document)
        {
            return new PostsViews(
                document.UserId,
                document.Views.Select(v => new View(v.PostId, v.Date))
            );
        }
    }
}

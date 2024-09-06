using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Posts.Core.Entities
{
    public class PostsViews
    {
        public Guid UserId { get; private set; }
        public IEnumerable<View> Views { get; private set; }

        public PostsViews(Guid userId, IEnumerable<View> views)
        {
            UserId = userId;
            Views = views ?? new List<View>();
        }

        public void AddView(Guid postId, DateTime date)
        {
            var viewList = new List<View>(Views)
            {
                new View(postId, date)
            };
            Views = viewList;
        }

        public void RemoveView(Guid postId)
        {
            var viewList = new List<View>(Views);
            var viewToRemove = viewList.Find(view => view.PostId == postId);
            if (viewToRemove != null)
            {
                viewList.Remove(viewToRemove);
                Views = viewList;
            }
        }
    }
}

using System;
using Paralax.Types;
using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    public class ViewDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public DateTime Date { get; set; }

        public static ViewDocument FromEntity(View view)
        {
            return new ViewDocument
            {
                Id = Guid.NewGuid(),
                PostId = view.PostId,
                Date = view.Date
            };
        }

        public View ToEntity()
        {
            return new View(PostId, Date);
        }
    }
}

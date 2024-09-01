using System;
using System.Collections.Generic;
using System.Linq;
using Convey.Types;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    public class UserProfileViewsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<UserProfileViewDocument> Views { get; set; } = new List<UserProfileViewDocument>();

        public static UserProfileViewsDocument FromEntity(UserProfileViews userProfileViews)
        {
            return new UserProfileViewsDocument
            {
                Id = Guid.NewGuid(),
                UserId = userProfileViews.UserId,
                Views = userProfileViews.Views.Select(UserProfileViewDocument.FromEntity).ToList()
            };
        }

        public UserProfileViews ToEntity()
        {
            return new UserProfileViews(UserId, Views.Select(view => view.ToEntity()));
        }
    }
}

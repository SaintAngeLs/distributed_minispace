using System;
using System.Collections.Generic;
using System.Linq;
using Paralax.Types;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    public class UserViewingProfilesDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<UserProfileViewDocument> ViewedProfiles { get; set; } = new List<UserProfileViewDocument>();

        public static UserViewingProfilesDocument FromEntity(UserViewingProfiles userViewingProfiles)
        {
            return new UserViewingProfilesDocument
            {
                Id = Guid.NewGuid(),
                UserId = userViewingProfiles.UserId,
                ViewedProfiles = userViewingProfiles.ViewedProfiles.Select(UserProfileViewDocument.FromEntity).ToList()
            };
        }

        public UserViewingProfiles ToEntity()
        {
            return new UserViewingProfiles(UserId, ViewedProfiles.Select(view => view.ToEntity()));
        }
    }
}
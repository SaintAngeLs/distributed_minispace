
using System.Linq;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    public static class UserProfileViewsExtensions
    {
        public static UserProfileViewsDocument AsDocument(this UserProfileViewsForUser entity)
        {
            return UserProfileViewsDocument.FromEntity(entity);
        }

        public static UserProfileViewsForUser AsEntity(this UserProfileViewsDocument document)
        {
            return document.ToEntity();
        }

        public static UserProfileViewDocument AsDocument(this UserProfileView view)
        {
            return UserProfileViewDocument.FromEntity(view);
        }

        public static UserProfileView AsEntity(this UserProfileViewDocument document)
        {
            return document.ToEntity();
        }

        public static UserViewingProfilesDocument AsDocument(this UserViewingProfiles entity)
        {
            return UserViewingProfilesDocument.FromEntity(entity);
        }

        public static UserViewingProfiles AsEntity(this UserViewingProfilesDocument document)
        {
            return document.ToEntity();
        }
    }
}

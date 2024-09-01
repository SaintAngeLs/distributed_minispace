using System.Linq;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    public static class UserProfileViewsExtensions
    {
        public static UserProfileViewsDocument AsDocument(this UserProfileViews entity)
        {
            return UserProfileViewsDocument.FromEntity(entity);
        }

        public static UserProfileViews AsEntity(this UserProfileViewsDocument document)
        {
            return document.ToEntity();
        }

        public static UserProfileViewDocument AsDocument(this View view)
        {
            return UserProfileViewDocument.FromEntity(view);
        }

        public static View AsEntity(this UserProfileViewDocument document)
        {
            return document.ToEntity();
        }
    }
}

using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Posts.Core.Entities;
using System.Collections.Generic;
using MongoDB.Driver;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public static class UserPostExtensions
    {
        public static UserPost AsEntity(this UserPostDocument document)
            => new UserPost(document.Id, document.UserId, document.UserPosts.Select(p => p.AsEntity()).ToList());

        public static UserPostDocument AsDocument(this UserPost entity)
            => new UserPostDocument
            {
                Id = entity.Id,
                UserId = entity.UserId,
                UserPosts = entity.UserPosts.Select(p => p.AsDocument()).ToList()
            };

        public static SortDefinition<UserPostDocument> ToSortDefinition(IEnumerable<string> sortBy, string direction)
        {
            var builder = Builders<UserPostDocument>.Sort;
            var sortDefinitions = new List<SortDefinition<UserPostDocument>>();

            foreach (var sortField in sortBy)
            {
                var fieldSort = direction.ToLower() == "desc" ? builder.Descending(sortField) : builder.Ascending(sortField);
                sortDefinitions.Add(fieldSort);
            }

            return builder.Combine(sortDefinitions);
        }
    }
}

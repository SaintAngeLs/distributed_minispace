using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Posts.Core.Entities;
using System.Collections.Generic;
using MongoDB.Driver;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public static class UserEventPostExtensions
    {
        public static UserEventPost AsEntity(this UserEventPostDocument document)
            => new UserEventPost(document.Id, document.UserId, document.UserEventPosts.Select(p => p.AsEntity()).ToList());

        public static UserEventPostDocument AsDocument(this UserEventPost entity)
            => new UserEventPostDocument
            {
                Id = entity.Id,
                UserId = entity.UserId,
                UserEventPosts = entity.UserEventPosts.Select(p => p.AsDocument()).ToList()
            };

        public static SortDefinition<UserEventPostDocument> ToSortDefinition(IEnumerable<string> sortBy, string direction)
        {
            var builder = Builders<UserEventPostDocument>.Sort;
            var sortDefinitions = new List<SortDefinition<UserEventPostDocument>>();

            foreach (var sortField in sortBy)
            {
                var fieldSort = direction.ToLower() == "desc" ? builder.Descending(sortField) : builder.Ascending(sortField);
                sortDefinitions.Add(fieldSort);
            }

            return builder.Combine(sortDefinitions);
        }



    }
}

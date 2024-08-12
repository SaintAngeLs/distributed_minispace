using System;
using System.Diagnostics.CodeAnalysis;
using Convey.Types;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class RatingDocument
    {
        public Guid StudentId { get; set; }
        public int Value { get; set; }

        public static RatingDocument FromEntity(Rating rating)
        {
            return new RatingDocument
            {
                StudentId = rating.StudentId,
                Value = rating.Value
            };
        }

        public Rating ToEntity()
        {
            return new Rating(StudentId, Value);
        }
    }
}
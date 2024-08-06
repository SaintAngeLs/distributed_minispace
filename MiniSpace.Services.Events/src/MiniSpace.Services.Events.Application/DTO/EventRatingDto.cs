using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class EventRatingDto
    {
        public Guid EventId { get; set; }
        public int TotalRatings { get; set; }
        public double AverageRating { get; set; }
        public IEnumerable<RatingDto> Ratings { get; set; }  // Add this property
    }
}

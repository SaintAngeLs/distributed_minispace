﻿using System;

namespace MiniSpace.Web.DTO
{
    public class EventRatingDto
    {
        public Guid EventId { get; set; }
        public int TotalRatings { get; set; }
        public double AverageRating { get; set; }
    }
}
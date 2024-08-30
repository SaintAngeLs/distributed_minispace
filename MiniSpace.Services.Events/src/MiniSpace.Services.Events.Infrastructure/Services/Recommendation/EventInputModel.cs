using System;
using Microsoft.ML.Data;

namespace MiniSpace.Services.Events.Infrastructure.Services.Recommendation
{
    public class EventInputModel
    {
        [LoadColumn(0)]
        public string Description { get; set; }

        [LoadColumn(1)]
        public float Label { get; set; }

        [LoadColumn(2)]
        public float TextLength { get; set; }

        [LoadColumn(3)]
        public float KeywordMatchCount { get; set; }

        [LoadColumn(4)]
        public float EventAgeDays { get; set; }

        [LoadColumn(5)]
        public float UserCommentScore { get; set; }

        [LoadColumn(6)]
        public float UserReactionScore { get; set; }

        [LoadColumn(7)]
        public float EducationMatchScore { get; set; }

        [LoadColumn(8)]
        public float WorkMatchScore { get; set; }
    }
}

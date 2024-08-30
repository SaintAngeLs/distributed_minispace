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
    }
}

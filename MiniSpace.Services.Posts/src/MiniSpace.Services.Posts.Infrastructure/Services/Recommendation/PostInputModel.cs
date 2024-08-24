using System;
using System.Collections.Generic;
using Microsoft.ML.Data;

namespace MiniSpace.Services.Posts.Infrastructure.Services.Recommendation
{
    public class PostInputModel
    {
        [LoadColumn(0)]
        public string TextContent { get; set; }

        [LoadColumn(1)]
        public float Label { get; set; }

        [LoadColumn(2)]
        public float TextLength { get; set; }

        [LoadColumn(3)]
        public float KeywordMatchCount { get; set; }

        [LoadColumn(4)]
        public float PostAgeDays { get; set; }
    }
}

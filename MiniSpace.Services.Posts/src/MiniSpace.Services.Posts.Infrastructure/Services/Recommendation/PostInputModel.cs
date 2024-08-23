using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace MiniSpace.Services.Posts.Infrastructure.Services.Recommendation
{
    public class PostInputModel
    {
        [LoadColumn(0)]
        public string TextContent { get; set; }

        [LoadColumn(1)]
        public float Label { get; set; }
    }
}
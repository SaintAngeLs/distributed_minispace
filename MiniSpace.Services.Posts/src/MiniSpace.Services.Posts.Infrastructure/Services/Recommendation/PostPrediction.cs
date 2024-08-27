using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace MiniSpace.Services.Posts.Infrastructure.Services.Recommendation
{
    public class PostPrediction
    {
        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
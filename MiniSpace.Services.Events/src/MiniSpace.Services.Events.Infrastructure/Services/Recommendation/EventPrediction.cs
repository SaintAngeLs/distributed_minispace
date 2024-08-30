using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace MiniSpace.Services.Events.Infrastructure.Services.Recommendation
{
    public class EventPrediction
    {
        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
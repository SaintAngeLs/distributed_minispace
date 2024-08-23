using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Infrastructure.Services.Recommendation;

namespace MiniSpace.Services.Posts.Infrastructure.Services
{
    public class PostRecommendationService : IPostRecommendationService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model; 

        public PostRecommendationService(MLContext mlContext, ITransformer model)
        {
            _mlContext = mlContext;
            _model = model;
        }

        public async Task<IEnumerable<(PostDto Post, double Score)>> RankPostsByUserInterestAsync(Guid userId, IEnumerable<PostDto> posts, IDictionary<string, double> userInterests)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<PostInputModel, PostPrediction>(_model);

            var rankedPosts = new List<(PostDto Post, double Score)>();

            foreach (var post in posts)
            {
                var input = new PostInputModel
                {
                    TextContent = post.TextContent
                };

                var prediction = predictionEngine.Predict(input);
                rankedPosts.Add((post, prediction.Score));
            }

            return rankedPosts.OrderByDescending(x => x.Score);
        }
    }
}

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

        public PostRecommendationService(MLContext mlContext)
        {
            _mlContext = mlContext;
        }

        public async Task<IEnumerable<(PostDto Post, double Score)>> RankPostsByUserInterestAsync(Guid userId, IEnumerable<PostDto> posts, IDictionary<string, double> userInterests)
        {
            // Step 1: Prepare the training data
            var trainingData = PrepareTrainingData(userInterests, posts);

            // Step 2: Train the model
            var model = TrainModel(trainingData);

            // Step 3: Use the trained model to rank posts
            var rankedPosts = new List<(PostDto Post, double Score)>();
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<PostInputModel, PostPrediction>(model);

            foreach (var post in posts)
            {
                var input = new PostInputModel
                {
                    TextContent = post.TextContent,
                    TextLength = post.TextContent.Length,
                    KeywordMatchCount = (float)userInterests.Where(ui => post.TextContent.Contains(ui.Key)).Sum(ui => ui.Value),
                    PostAgeDays = (post.PublishDate.HasValue ? (float)(DateTime.UtcNow - post.PublishDate.Value).TotalDays : 0)
                };

                var prediction = predictionEngine.Predict(input);
                rankedPosts.Add((post, prediction.Score));
            }

            return rankedPosts.OrderByDescending(x => x.Score);
        }

        private IDataView PrepareTrainingData(IDictionary<string, double> userInterests, IEnumerable<PostDto> posts)
        {
            var inputData = posts.Select(post =>
            {
                var keywordMatches = userInterests.Where(ui => post.TextContent.Contains(ui.Key)).Sum(ui => ui.Value);
                return new PostInputModel
                {
                    TextContent = post.TextContent,
                    Label = (float)keywordMatches, 
                    TextLength = post.TextContent.Length,
                    KeywordMatchCount = (float)keywordMatches, 
                    PostAgeDays = (post.PublishDate.HasValue ? (float)(DateTime.UtcNow - post.PublishDate.Value).TotalDays : 0) 
                };
            });

            return _mlContext.Data.LoadFromEnumerable(inputData);
        }

        private ITransformer TrainModel(IDataView trainingData)
        {
            var dataProcessPipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(PostInputModel.TextContent));

            var trainer = _mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            var model = trainingPipeline.Fit(trainingData);
            return model;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Services;

namespace MiniSpace.Services.Events.Infrastructure.Services.Recommendation
{
    public class EventRecommendationService : IEventRecommendationService
    {
        private readonly MLContext _mlContext;

        public EventRecommendationService(MLContext mlContext)
        {
            _mlContext = mlContext;
        }

        public async Task<IEnumerable<(EventDto Event, double Score)>> RankEventsByUserInterestAsync(Guid userId, IEnumerable<EventDto> events, IDictionary<string, double> userInterests)
        {
            // Prepare the training data
            var trainingData = PrepareTrainingData(userInterests, events);

            // Train the model
            var model = TrainModel(trainingData);

            // Use the trained model to rank events
            var rankedEvents = new List<(EventDto Event, double Score)>();
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<EventInputModel, EventPrediction>(model);

            foreach (var eventItem in events)
            {
                var input = CreateInputModel(eventItem, userInterests);
                var prediction = predictionEngine.Predict(input);
                rankedEvents.Add((eventItem, prediction.Score));
            }

            return rankedEvents.OrderByDescending(x => x.Score);
        }

        private IDataView PrepareTrainingData(IDictionary<string, double> userInterests, IEnumerable<EventDto> events)
        {
            var inputData = events.Select(eventItem =>
            {
                var keywordMatches = userInterests.Where(ui => eventItem.Description.Contains(ui.Key)).Sum(ui => ui.Value);
                var inputModel = CreateInputModel(eventItem, userInterests);
                inputModel.Label = (float)keywordMatches;  // Use keyword match as the label for training

                return inputModel;
            });

            return _mlContext.Data.LoadFromEnumerable(inputData);
        }

        private EventInputModel CreateInputModel(EventDto eventItem, IDictionary<string, double> userInterests)
        {
            var keywordMatches = userInterests.Where(ui => eventItem.Description.Contains(ui.Key)).Sum(ui => ui.Value);
            return new EventInputModel
            {
                Description = eventItem.Description,
                TextLength = eventItem.Description.Length,
                KeywordMatchCount = (float)keywordMatches,
                EventAgeDays = eventItem.StartDate != DateTime.MinValue ? (float)(DateTime.UtcNow - eventItem.StartDate).TotalDays : 0,
                UserCommentScore = GetUserCommentScore(eventItem),
                UserReactionScore = GetUserReactionScore(eventItem),
                EducationMatchScore = GetEducationMatchScore(eventItem),
                WorkMatchScore = GetWorkMatchScore(eventItem)
            };
        }

        private ITransformer TrainModel(IDataView trainingData)
        {
            var dataProcessPipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(EventInputModel.Description))
                .Append(_mlContext.Transforms.Concatenate("Features", nameof(EventInputModel.TextLength),
                                                                   nameof(EventInputModel.KeywordMatchCount),
                                                                   nameof(EventInputModel.EventAgeDays),
                                                                   nameof(EventInputModel.UserCommentScore),
                                                                   nameof(EventInputModel.UserReactionScore),
                                                                   nameof(EventInputModel.EducationMatchScore),
                                                                   nameof(EventInputModel.WorkMatchScore)));

            var trainer = _mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            return trainingPipeline.Fit(trainingData);
        }

        private float GetUserCommentScore(EventDto eventItem)
        {
            // Stub: Implement actual logic to calculate user comment score
            return 0.5f;
        }

        private float GetUserReactionScore(EventDto eventItem)
        {
            // Stub: Implement actual logic to calculate user reaction score
            return 0.3f;
        }

        private float GetEducationMatchScore(EventDto eventItem)
        {
            // Stub: Implement actual logic to calculate education match score
            return 0.7f;
        }

        private float GetWorkMatchScore(EventDto eventItem)
        {
            // Stub: Implement actual logic to calculate work match score
            return 0.6f;
        }
    }
}

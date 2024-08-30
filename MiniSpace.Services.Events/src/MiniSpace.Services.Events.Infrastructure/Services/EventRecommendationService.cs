using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Infrastructure.Services.Recommendation;

namespace MiniSpace.Services.Events.Infrastructure.Services
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
            // Step 1: Prepare the training data
            var trainingData = PrepareTrainingData(userInterests, events);

            // Step 2: Train the model
            var model = TrainModel(trainingData);

            // Step 3: Use the trained model to rank events
            var rankedEvents = new List<(EventDto Event, double Score)>();
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<EventInputModel, EventPrediction>(model);

            foreach (var eventItem in events)
            {
                var input = new EventInputModel
                {
                    Description = eventItem.Description,
                    TextLength = eventItem.Description.Length,
                    KeywordMatchCount = (float)userInterests.Where(ui => eventItem.Description.Contains(ui.Key)).Sum(ui => ui.Value),
                    EventAgeDays = eventItem.StartDate != DateTime.MinValue 
                                    ? (float)(DateTime.UtcNow - eventItem.StartDate).TotalDays 
                                    : 0

                };

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
                return new EventInputModel
                {
                    Description = eventItem.Description,
                    Label = (float)keywordMatches,
                    TextLength = eventItem.Description.Length,
                    KeywordMatchCount = (float)keywordMatches,
                    EventAgeDays = eventItem.StartDate != DateTime.MinValue 
                                ? (float)(DateTime.UtcNow - eventItem.StartDate).TotalDays 
                                : 0

                };
            });

            return _mlContext.Data.LoadFromEnumerable(inputData);
        }

        private ITransformer TrainModel(IDataView trainingData)
        {
            var dataProcessPipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(EventInputModel.Description));

            var trainer = _mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            var model = trainingPipeline.Fit(trainingData);
            return model;
        }
    }
}

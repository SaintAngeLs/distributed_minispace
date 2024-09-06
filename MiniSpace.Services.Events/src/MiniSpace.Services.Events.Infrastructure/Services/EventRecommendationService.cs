using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.ML.Data;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Services;

namespace MiniSpace.Services.Events.Infrastructure.Services.Recommendation
{
    public class EventRecommendationService : IEventRecommendationService
    {
        private readonly MLContext _mlContext;
        private readonly ILogger<EventRecommendationService> _logger;

        public EventRecommendationService(ILogger<EventRecommendationService> logger)
        {
            _mlContext = new MLContext();
            _logger = logger;
        }

        public IEnumerable<EventDto> RankEventsByUserInterest(Guid userId, IEnumerable<EventDto> events, IEnumerable<string> userInterests)
        {
            _logger.LogInformation("Starting RankEventsByUserInterest method.");
            var stopwatch = Stopwatch.StartNew();

            // Convert user interests to HashSet for quick lookups
            var userInterestsSet = new HashSet<string>(userInterests, StringComparer.OrdinalIgnoreCase);

            // Prepare input data and filter out unlikely matches early to reduce the data size
            var inputData = events
                .Where(e => userInterestsSet.Any(interest => e.Description.Contains(interest, StringComparison.OrdinalIgnoreCase)))
                .Select(e => CreateInputModel(e, userInterestsSet))
                .ToArray();

            // Return early if no events match the user's interests
            if (inputData.Length == 0)
            {
                _logger.LogInformation("No events matched user interests. Returning empty list.");
                return Enumerable.Empty<EventDto>();
            }

            // Train model dynamically for the specific user
            var userModel = TrainUserModel(inputData);
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<EventInputModel, EventPrediction>(userModel);

            // Score events
            var scoredEvents = inputData
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .Select(input => (Event: events.First(e => e.Id.ToString() == input.EventId), Score: predictionEngine.Predict(input).Score))
                .OrderByDescending(result => result.Score)
                .Select(result => result.Event)
                .ToList();

            _logger.LogInformation("Completed RankEventsByUserInterest method in {TotalElapsedMilliseconds} ms.", stopwatch.ElapsedMilliseconds);
            return scoredEvents;
        }

        // Asynchronous version of the method
        public async Task<IEnumerable<EventDto>> RankEventsByUserInterestAsync(Guid userId, IEnumerable<EventDto> events, IEnumerable<string> userInterests)
        {
            return await Task.Run(() => RankEventsByUserInterest(userId, events, userInterests));
        }

        private EventInputModel CreateInputModel(EventDto eventItem, HashSet<string> userInterests)
        {
            var keywordMatches = userInterests.Count(interest => eventItem.Description.Contains(interest, StringComparison.OrdinalIgnoreCase));
            return new EventInputModel
            {
                EventId = eventItem.Id.ToString(),
                TextLength = eventItem.Description?.Length ?? 0,
                KeywordMatchCount = keywordMatches,
                EventAgeDays = eventItem.StartDate != DateTime.MinValue ? (float)(DateTime.UtcNow - eventItem.StartDate).TotalDays : 0,
                Label = keywordMatches // Use keyword matches as the label
            };
        }

        private ITransformer TrainUserModel(EventInputModel[] inputData)
        {
            var trainingData = _mlContext.Data.LoadFromEnumerable(inputData);

            var dataProcessPipeline = _mlContext.Transforms.Concatenate("Features", nameof(EventInputModel.TextLength),
                                                                       nameof(EventInputModel.KeywordMatchCount),
                                                                       nameof(EventInputModel.EventAgeDays));

            var trainer = _mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            // Train the model dynamically for the user
            return trainingPipeline.Fit(trainingData);
        }
    }
}


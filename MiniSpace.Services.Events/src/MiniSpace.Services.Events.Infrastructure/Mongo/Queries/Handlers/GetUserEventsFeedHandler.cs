using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Services.Clients;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Core.Wrappers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Queries.Handlers
{
    public class GetUserEventsFeedHandler : IQueryHandler<GetUserEventsFeed, PagedResponse<EventDto>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventRecommendationService _eventRecommendationService;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IAppContext _appContext;
        private readonly ILogger<GetUserEventsFeedHandler> _logger;

        public GetUserEventsFeedHandler(
            IEventRepository eventRepository,
            IEventRecommendationService eventRecommendationService,
            IStudentsServiceClient studentsServiceClient,
            IAppContext appContext,
            ILogger<GetUserEventsFeedHandler> logger)
        {
            _eventRepository = eventRepository;
            _eventRecommendationService = eventRecommendationService;
            _studentsServiceClient = studentsServiceClient;
            _appContext = appContext;
            _logger = logger;
        }

        public async Task<PagedResponse<EventDto>> HandleAsync(GetUserEventsFeed query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetUserEventsFeed query: {Query}", JsonConvert.SerializeObject(query));

            // Fetch user data and events in parallel
            var userTask = _studentsServiceClient.GetStudentByIdAsync(query.UserId);
            var eventsTask = _eventRepository.BrowseEventsAsync(
                query.PageNumber,
                query.PageSize,
                name: string.Empty,
                organizer: string.Empty,
                dateFrom: default,
                dateTo: default,
                category: null,
                state: null,
                organizations: Enumerable.Empty<Guid>(),
                friends: Enumerable.Empty<Guid>(),
                friendsEngagementType: null,
                sortBy: new List<string> { query.SortBy },
                direction: query.Direction
            );

            await Task.WhenAll(userTask, eventsTask);

            var user = userTask.Result;
            var (events, _, _, _, _) = eventsTask.Result;

            if (user == null)
            {
                return new PagedResponse<EventDto>(Enumerable.Empty<EventDto>(), query.PageNumber, query.PageSize, 0);
            }

            var studentId = _appContext.Identity.Id;
            var eventDtos = events.Select(e => e.AsDto(studentId)).ToList();

            // Rank events using the recommendation service
            var rankedEvents = _eventRecommendationService.RankEventsByUserInterest(query.UserId, eventDtos, user.Interests);

            if (!rankedEvents.Any())
            {
                // If no ranked events, fetch all events
                _logger.LogInformation("No ranked events found, loading all events.");
                var allEventsTask = _eventRepository.BrowseEventsAsync(
                    query.PageNumber,
                    query.PageSize,
                    name: string.Empty,
                    organizer: string.Empty,
                    dateFrom: default,
                    dateTo: default,
                    category: null,
                    state: null,
                    organizations: Enumerable.Empty<Guid>(),
                    friends: Enumerable.Empty<Guid>(),
                    friendsEngagementType: null,
                    sortBy: new List<string> { query.SortBy },
                    direction: query.Direction
                );

                await allEventsTask;

                var (allEvents, _, _, _, _) = allEventsTask.Result;
                eventDtos = allEvents.Select(e => e.AsDto(studentId)).ToList();
                rankedEvents = eventDtos; // Use all events as the ranked events
            }

            // Paginate the ranked events
            var pagedEvents = rankedEvents
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            var response = new PagedResponse<EventDto>(pagedEvents, query.PageNumber, query.PageSize, rankedEvents.Count());

            // Log the response for debugging purposes
            var jsonResponse = JsonConvert.SerializeObject(response, Formatting.Indented);
            _logger.LogInformation("Response JSON: {JsonResponse}", jsonResponse);

            return response;
        }
    }
}

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
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Queries.Handlers
{
    public class GetUserEventsFeedHandler : IQueryHandler<GetUserEventsFeed, PagedResponse<EventDto>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventRecommendationService _eventRecommendationService;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IUserCommentsHistoryRepository _userCommentsHistoryRepository;
        private readonly IUserReactionsHistoryRepository _userReactionsHistoryRepository;
        private readonly IAppContext _appContext;
        private readonly ILogger<GetUserEventsFeedHandler> _logger;

        public GetUserEventsFeedHandler(
            IEventRepository eventRepository,
            IEventRecommendationService eventRecommendationService,
            IStudentsServiceClient studentsServiceClient,
            IUserCommentsHistoryRepository userCommentsHistoryRepository,
            IUserReactionsHistoryRepository userReactionsHistoryRepository,
            IAppContext appContext,
            ILogger<GetUserEventsFeedHandler> logger)
        {
            _eventRepository = eventRepository;
            _eventRecommendationService = eventRecommendationService;
            _studentsServiceClient = studentsServiceClient;
            _userCommentsHistoryRepository = userCommentsHistoryRepository;
            _userReactionsHistoryRepository = userReactionsHistoryRepository;
            _appContext = appContext;
            _logger = logger;
        }

        public async Task<PagedResponse<EventDto>> HandleAsync(GetUserEventsFeed query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetUserEventsFeed query: {Query}", JsonConvert.SerializeObject(query));

            // Fetch user data
            var user = await _studentsServiceClient.GetStudentByIdAsync(query.UserId);
            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found.", query.UserId);
                return new PagedResponse<EventDto>(Enumerable.Empty<EventDto>(), query.PageNumber, query.PageSize, 0);
            }

            // Fetch user comments and reactions
            var userComments = await _userCommentsHistoryRepository.GetUserCommentsAsync(query.UserId);
            var userReactions = await _userReactionsHistoryRepository.GetUserReactionsAsync(query.UserId);

            // Analyze user's interests based on their profile and interactions
            var userInterests = AnalyzeUserInterests(user, userComments, userReactions);

            // Fetch events to be ranked
            var (events, pageNumber, pageSize, totalPages, totalElements) = await _eventRepository.BrowseEventsAsync(
                pageNumber: query.PageNumber,
                pageSize: query.PageSize,
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

            if (events == null || !events.Any())
            {
                return new PagedResponse<EventDto>(Enumerable.Empty<EventDto>(), query.PageNumber, query.PageSize, 0);
            }

            var studentId = _appContext.Identity.Id;
            var eventDtos = events.Select(e => e.AsDto(studentId)).ToList();

            // Rank events using the recommendation service
            var rankedEvents = await _eventRecommendationService.RankEventsByUserInterestAsync(query.UserId, eventDtos, userInterests);

            // Paginate the ranked events
            var pagedEvents = rankedEvents
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            _logger.LogInformation("User {UserId} event feed generated with {EventCount} events.", query.UserId, pagedEvents.Count);

            return new PagedResponse<EventDto>(pagedEvents.Select(e => e.Event), query.PageNumber, query.PageSize, rankedEvents.Count());
        }

        private IDictionary<string, double> AnalyzeUserInterests(UserFromServiceDto user, IEnumerable<Comment> comments, IEnumerable<Reaction> reactions)
        {
            var interestKeywords = new Dictionary<string, double>();

            // Analyze profile data
            if (user.Interests != null)
            {
                foreach (var interest in user.Interests)
                {
                    if (interestKeywords.ContainsKey(interest))
                    {
                        interestKeywords[interest] += 1;
                    }
                    else
                    {
                        interestKeywords[interest] = 1;
                    }
                }
            }

            if (user.Education != null)
            {
                foreach (var education in user.Education)
                {
                    var keywords = new[] { education.Degree, education.InstitutionName };
                    foreach (var keyword in keywords)
                    {
                        if (interestKeywords.ContainsKey(keyword))
                        {
                            interestKeywords[keyword] += 1;
                        }
                        else
                        {
                            interestKeywords[keyword] = 1;
                        }
                    }
                }
            }

            if (user.Work != null)
            {
                foreach (var work in user.Work)
                {
                    var keywords = new[] { work.Position, work.Company };
                    foreach (var keyword in keywords)
                    {
                        if (interestKeywords.ContainsKey(keyword))
                        
                        {
                            interestKeywords[keyword] += 1;
                        }
                        else
                        {
                            interestKeywords[keyword] = 1;
                        }
                    }
                }
            }

            // Analyze comments
            foreach (var comment in comments)
            {
                var words = comment.TextContent.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    if (interestKeywords.ContainsKey(word))
                    {
                        interestKeywords[word] += 1;
                    }
                    else
                    {
                        interestKeywords[word] = 1;
                    }
                }
            }

            // Analyze reactions
            foreach (var reaction in reactions)
            {
                if (interestKeywords.ContainsKey(reaction.Type))
                {
                    interestKeywords[reaction.Type] += 1;
                }
                else
                {
                    interestKeywords[reaction.Type] = 1;
                }
            }

            // Normalize the scores
            var total = interestKeywords.Values.Sum();
            if (total > 0)
            {
                return interestKeywords.ToDictionary(kvp => kvp.Key, kvp => kvp.Value / total);
            }

            return interestKeywords;
        }
    }
}

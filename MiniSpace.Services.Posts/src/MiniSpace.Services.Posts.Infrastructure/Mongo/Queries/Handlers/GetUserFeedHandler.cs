using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.Queries;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Core.Wrappers;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Requests;
using Newtonsoft.Json;
using MiniSpace.Services.Posts.Application.Services.Clients;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Queries.Handlers
{
    public class GetUserFeedHandler : IQueryHandler<GetUserFeed, PagedResponse<PostDto>>
    {
        private readonly IPostsService _postsService;
        private readonly IUserCommentsHistoryRepository _userCommentsHistoryRepository;
        private readonly IUserReactionsHistoryRepository _userReactionsHistoryRepository;
        private readonly IPostRecommendationService _postRecommendationService;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly ILogger<GetUserFeedHandler> _logger;

        public GetUserFeedHandler(
            IPostsService postsService,
            IUserCommentsHistoryRepository userCommentsHistoryRepository,
            IUserReactionsHistoryRepository userReactionsHistoryRepository,
            IPostRecommendationService postRecommendationService,
            IStudentsServiceClient studentsServiceClient,
            ILogger<GetUserFeedHandler> logger)
        {
            _postsService = postsService;
            _userCommentsHistoryRepository = userCommentsHistoryRepository;
            _userReactionsHistoryRepository = userReactionsHistoryRepository;
            _postRecommendationService = postRecommendationService;
            _studentsServiceClient = studentsServiceClient;
            _logger = logger;
        }

        public async Task<PagedResponse<PostDto>> HandleAsync(GetUserFeed query, CancellationToken cancellationToken)
        {
             Console.WriteLine($"Query Parameters - UserId: {query.UserId}, PageNumber: {query.PageNumber}, PageSize: {query.PageSize}, SortBy: {query.SortBy}, Direction: {query.Direction}");
    


            var user = await _studentsServiceClient.GetStudentByIdAsync(query.UserId);

    //         var serializedUser = JsonConvert.SerializeObject(user, Formatting.Indented);
    // Console.WriteLine($"Retrieved User Object: {serializedUser}");

            var allPostsRequest = new BrowseRequest
            {
                SortBy = new List<string> { query.SortBy },
                Direction = query.Direction
            };

            var allPostsResult = await _postsService.BrowsePostsAsync(allPostsRequest);

    //         var serializedAllPostsResult = JsonConvert.SerializeObject(allPostsResult, Formatting.Indented);
    // Console.WriteLine($"Retrieved All posts Object: {serializedAllPostsResult}");

            if (allPostsResult == null || !allPostsResult.Items.Any())
            {
                return new PagedResponse<PostDto>(Enumerable.Empty<PostDto>(), query.PageNumber, query.PageSize, 0);
            }

            var userComments = await _userCommentsHistoryRepository.GetUserCommentsAsync(query.UserId);
            var userReactions = await _userReactionsHistoryRepository.GetUserReactionsAsync(query.UserId);

            var userInterests = AnalyzeUserInteractions(user, userComments, userReactions);

            IEnumerable<(PostDto Post, double Score)> rankedPosts;

            if (!userInterests.Any() && !userComments.Any() && !userReactions.Any())
            {
                _logger.LogInformation("User {UserId} has no interactions or defined interests, generating a random feed.", query.UserId);
                rankedPosts = GenerateRandomFeed(allPostsResult.Items);
            }
            else
            {
                rankedPosts = await _postRecommendationService.RankPostsByUserInterestAsync(query.UserId, allPostsResult.Items, userInterests);
            }

            var combinedPosts = CombineRankedAndUnrankedPosts(rankedPosts, allPostsResult.Items);

//             var serializedCombinedPosts = JsonConvert.SerializeObject(combinedPosts, Formatting.Indented);

// // Output the serialized result to the console
// Console.WriteLine($"Combined Posts Object: {serializedCombinedPosts}");

            var totalPosts = combinedPosts.Count();
_logger.LogInformation("Total posts: {TotalPosts}, PageNumber: {PageNumber}, PageSize: {PageSize}, Skip: {Skip}", 
    totalPosts, query.PageNumber, query.PageSize, (query.PageNumber - 1) * query.PageSize);

var pagedPosts = combinedPosts
    .Skip((query.PageNumber - 1) * query.PageSize)
    .Take(query.PageSize)
    .ToList();


            _logger.LogInformation("User {UserId} feed generated with {PostCount} posts.", query.UserId, pagedPosts.Count);

            return new PagedResponse<PostDto>(pagedPosts, query.PageNumber, query.PageSize, combinedPosts.Count());
        }

        private IDictionary<string, double> AnalyzeUserInteractions(UserDto user, IEnumerable<Comment> userComments, IEnumerable<Reaction> userReactions)
        {
            var interestKeywords = new Dictionary<string, double>();

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

            if (user.Languages != null)
            {
                foreach (var language in user.Languages)
                {
                    if (interestKeywords.ContainsKey(language))
                    {
                        interestKeywords[language] += 1;
                    }
                    else
                    {
                        interestKeywords[language] = 1;
                    }
                }
            }

            foreach (var comment in userComments)
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

            foreach (var reaction in userReactions)
            {
                var reactionType = reaction.Type;
                if (interestKeywords.ContainsKey(reactionType))
                {
                    interestKeywords[reactionType] += 1;
                }
                else
                {
                    interestKeywords[reactionType] = 1;
                }
            }

            var total = interestKeywords.Values.Sum();
            if (total > 0)
            {
                var normalizedInterests = interestKeywords.ToDictionary(kvp => kvp.Key, kvp => kvp.Value / total);
                _logger.LogInformation("Inferred user interests with coefficients: {Interests}", string.Join(", ", normalizedInterests.Select(kvp => $"{kvp.Key}: {kvp.Value:F2}")));
                return normalizedInterests;
            }

            return interestKeywords; 
        }

        private IEnumerable<(PostDto Post, double Score)> GenerateRandomFeed(IEnumerable<PostDto> allPosts)
        {
            var random = new Random();
            return allPosts
                .OrderBy(p => random.NextDouble())
                .Select(p => (p, Score: random.NextDouble())) 
                .Take(100); 
        }


        private IEnumerable<PostDto> CombineRankedAndUnrankedPosts(IEnumerable<(PostDto Post, double Score)> rankedPosts, IEnumerable<PostDto> allPosts)
        {
            var rankedPostIds = rankedPosts.Select(rp => rp.Post.Id).ToHashSet();
            var unrankedPosts = allPosts.Where(p => !rankedPostIds.Contains(p.Id));

            return rankedPosts.Select(rp => rp.Post)
                              .Concat(unrankedPosts.OrderByDescending(p => p.PublishDate));  
        }
    }
}

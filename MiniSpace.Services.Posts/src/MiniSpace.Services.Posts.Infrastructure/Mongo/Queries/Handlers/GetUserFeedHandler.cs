using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
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
            _logger.LogInformation("Handling GetUserFeed query: {Query}", JsonConvert.SerializeObject(query));

            // Retrieve the user details
            var user = await _studentsServiceClient.GetStudentByIdAsync(query.UserId);

            // Step 1: Retrieve all posts without pagination (we'll handle ranking and pagination manually)
            var allPostsRequest = new BrowseRequest
            {
                SortBy = new List<string> { query.SortBy },
                Direction = query.Direction
            };

            var allPostsResult = await _postsService.BrowsePostsAsync(allPostsRequest);

            if (allPostsResult == null || !allPostsResult.Items.Any())
            {
                return new PagedResponse<PostDto>(Enumerable.Empty<PostDto>(), query.PageNumber, query.PageSize, 0);
            }

            // Step 2: Retrieve user interactions (comments and reactions)
            var userComments = await _userCommentsHistoryRepository.GetUserCommentsAsync(query.UserId);
            var userReactions = await _userReactionsHistoryRepository.GetUserReactionsAsync(query.UserId);

            // Step 3: Analyze user interactions to infer interests and calculate coefficients
            var userInterests = AnalyzeUserInteractions(user, userComments, userReactions);

            // Step 4: Rank all posts by relevance to user interests
            IEnumerable<(PostDto Post, double Score)> rankedPosts;

            // If the user has no interests, comments, or reactions, generate a random feed
            if (!userInterests.Any() && !userComments.Any() && !userReactions.Any())
            {
                _logger.LogInformation("User {UserId} has no interactions or defined interests, generating a random feed.", query.UserId);
                rankedPosts = GenerateRandomFeed(allPostsResult.Items);
            }
            else
            {
                rankedPosts = await _postRecommendationService.RankPostsByUserInterestAsync(query.UserId, allPostsResult.Items, userInterests);
            }

            // Step 5: Combine ranked posts with remaining posts, prioritizing more relevant posts
            var combinedPosts = CombineRankedAndUnrankedPosts(rankedPosts, allPostsResult.Items);

            // Step 6: Paginate the combined posts
            var pagedPosts = combinedPosts
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            // Log the result
            _logger.LogInformation("User {UserId} feed generated with {PostCount} posts.", query.UserId, pagedPosts.Count);

            return new PagedResponse<PostDto>(pagedPosts, query.PageNumber, query.PageSize, combinedPosts.Count());
        }

        private IDictionary<string, double> AnalyzeUserInteractions(UserDto user, IEnumerable<Comment> userComments, IEnumerable<Reaction> userReactions)
        {
            var interestKeywords = new Dictionary<string, double>();

            // Include user interests in the analysis if they exist
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

            // Include user languages in the analysis if they exist
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

            // Analyze user comments
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

            // Analyze user reactions
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

            // Normalize coefficients to sum to 1 if there are any
            var total = interestKeywords.Values.Sum();
            if (total > 0)
            {
                var normalizedInterests = interestKeywords.ToDictionary(kvp => kvp.Key, kvp => kvp.Value / total);
                _logger.LogInformation("Inferred user interests with coefficients: {Interests}", string.Join(", ", normalizedInterests.Select(kvp => $"{kvp.Key}: {kvp.Value:F2}")));
                return normalizedInterests;
            }

            return interestKeywords; // This may be empty if no interests, languages, comments, or reactions
        }

        private IEnumerable<(PostDto Post, double Score)> GenerateRandomFeed(IEnumerable<PostDto> allPosts)
        {
            var random = new Random();
            return allPosts
                .OrderBy(p => random.NextDouble())
                .Select(p => (p, Score: random.NextDouble())) // Assign a random score for ranking
                .Take(100); // Limit to a reasonable number to prevent overwhelming the user
        }



        private IEnumerable<PostDto> CombineRankedAndUnrankedPosts(IEnumerable<(PostDto Post, double Score)> rankedPosts, IEnumerable<PostDto> allPosts)
        {
            // Order posts by the relevance score first and then by publish date if the score is equal
            var rankedPostIds = rankedPosts.Select(rp => rp.Post.Id).ToHashSet();
            var unrankedPosts = allPosts.Where(p => !rankedPostIds.Contains(p.Id));

            return rankedPosts.Select(rp => rp.Post)
                              .Concat(unrankedPosts.OrderByDescending(p => p.PublishDate));  // Fallback to publish date for unranked posts
        }
    }
}
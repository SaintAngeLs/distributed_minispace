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

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Queries.Handlers
{
    public class GetUserFeedHandler : IQueryHandler<GetUserFeed, PagedResponse<PostDto>>
    {
        private readonly IPostsService _postsService;
        private readonly IUserCommentsHistoryRepository _userCommentsHistoryRepository;
        private readonly IUserReactionsHistoryRepository _userReactionsHistoryRepository;
        private readonly IPostRecommendationService _postRecommendationService;
        private readonly ILogger<GetUserFeedHandler> _logger;

        public GetUserFeedHandler(
            IPostsService postsService,
            IUserCommentsHistoryRepository userCommentsHistoryRepository,
            IUserReactionsHistoryRepository userReactionsHistoryRepository,
            IPostRecommendationService postRecommendationService,
            ILogger<GetUserFeedHandler> logger)
        {
            _postsService = postsService;
            _userCommentsHistoryRepository = userCommentsHistoryRepository;
            _userReactionsHistoryRepository = userReactionsHistoryRepository;
            _postRecommendationService = postRecommendationService;
            _logger = logger;
        }

        public async Task<PagedResponse<PostDto>> HandleAsync(GetUserFeed query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetUserFeed query: {Query}", JsonConvert.SerializeObject(query));

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
            var userInterests = AnalyzeUserInteractions(userComments, userReactions);

            // Step 4: Rank all posts by relevance to user interests
            var rankedPosts = await _postRecommendationService.RankPostsByUserInterestAsync(query.UserId, allPostsResult.Items, userInterests);

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

        private IDictionary<string, double> AnalyzeUserInteractions(IEnumerable<Comment> userComments, IEnumerable<Reaction> userReactions)
        {
            var interestKeywords = new Dictionary<string, double>();

            // Analyze user comments
            foreach (var comment in userComments)
            {
                var words = comment.TextContent.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    if (interestKeywords.ContainsKey(word))
                    {
                        interestKeywords[word] += 1; // Increase weight for recurring words
                    }
                    else
                    {
                        interestKeywords[word] = 1; // Initial weight
                    }
                }
            }

            // Analyze user reactions
            foreach (var reaction in userReactions)
            {
                var reactionType = reaction.Type; // Use 'Type' instead of 'ReactionType'
                if (interestKeywords.ContainsKey(reactionType))
                {
                    interestKeywords[reactionType] += 1; // Increase weight for recurring reactions
                }
                else
                {
                    interestKeywords[reactionType] = 1; // Initial weight for this reaction type
                }
            }

            // Normalize coefficients to sum to 1
            var total = interestKeywords.Values.Sum();
            var normalizedInterests = interestKeywords.ToDictionary(kvp => kvp.Key, kvp => kvp.Value / total);

            _logger.LogInformation("Inferred user interests with coefficients: {Interests}", string.Join(", ", normalizedInterests.Select(kvp => $"{kvp.Key}: {kvp.Value:F2}")));

            return normalizedInterests;
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

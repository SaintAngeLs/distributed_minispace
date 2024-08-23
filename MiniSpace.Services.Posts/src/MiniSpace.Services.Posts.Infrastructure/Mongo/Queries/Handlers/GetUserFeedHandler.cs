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

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Queries.Handlers
{
    public class GetUserFeedHandler : IQueryHandler<GetUserFeed, PagedResponse<PostDto>>
    {
        private readonly IPostsService _postsService;
        private readonly IUserCommentsHistoryRepository _userCommentsHistoryRepository;
        private readonly IPostRecommendationService _postRecommendationService;
        private readonly ILogger<GetUserFeedHandler> _logger;

        public GetUserFeedHandler(IPostsService postsService, IUserCommentsHistoryRepository userCommentsHistoryRepository, IPostRecommendationService postRecommendationService, ILogger<GetUserFeedHandler> logger)
        {
            _postsService = postsService;
            _userCommentsHistoryRepository = userCommentsHistoryRepository;
            _postRecommendationService = postRecommendationService;
            _logger = logger;
        }

        public async Task<PagedResponse<PostDto>> HandleAsync(GetUserFeed query, CancellationToken cancellationToken)
        {
            // Step 1: Retrieve user comments history
            var userComments = await _userCommentsHistoryRepository.GetUserCommentsAsync(query.UserId);
            
            // Step 2: Analyze the user's comments to infer interests and calculate coefficients
            var userInterests = AnalyzeUserComments(userComments);

            // Step 3: Fetch posts
            var allPostsRequest = new BrowseRequest
            {
                UserId = query.UserId,
                PageNumber = 1,
                PageSize = int.MaxValue, 
            };

            var allPostsResult = await _postsService.BrowsePostsAsync(allPostsRequest);

            // Step 4: Rank posts by relevance to user interests using the ML service
            var rankedPosts = await _postRecommendationService.RankPostsByUserInterestAsync(query.UserId, allPostsResult.Items, userInterests);

            // Step 5: Paginate the results
            var pagedRankedPosts = rankedPosts
                .OrderByDescending(x => x.Score)  // Order by descending score (most relevant first)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => x.Post); 

            // Log the result
            _logger.LogInformation("User {UserId} feed generated with {PostCount} posts.", query.UserId, pagedRankedPosts.Count());

            return new PagedResponse<PostDto>(pagedRankedPosts, query.PageNumber, query.PageSize, rankedPosts.Count());
        }

        private IDictionary<string, double> AnalyzeUserComments(IEnumerable<Comment> userComments)
        {
            var interestKeywords = new Dictionary<string, double>();

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

            // Normalize coefficients to sum to 1
            var total = interestKeywords.Values.Sum();
            var normalizedInterests = interestKeywords.ToDictionary(kvp => kvp.Key, kvp => kvp.Value / total);

            _logger.LogInformation("Inferred user interests with coefficients: {Interests}", string.Join(", ", normalizedInterests.Select(kvp => $"{kvp.Key}: {kvp.Value:F2}")));

            return normalizedInterests;
        }
    }
}

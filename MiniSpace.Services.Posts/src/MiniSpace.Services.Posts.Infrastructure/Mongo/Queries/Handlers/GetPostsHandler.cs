using Convey.CQRS.Queries;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.Queries;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Requests;
using MiniSpace.Services.Posts.Core.Wrappers;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Queries.Handlers
{
    public class GetPostsHandler : IQueryHandler<GetPosts, PagedResponse<PostDto>>
    {
        private readonly IPostsService _postsService;
        private readonly ILogger<GetPostsHandler> _logger;

        public GetPostsHandler(IPostsService postsService, ILogger<GetPostsHandler> logger)
        {
            _postsService = postsService;
            _logger = logger;
        }

        public async Task<PagedResponse<PostDto>> HandleAsync(GetPosts query, CancellationToken cancellationToken)
        {
            var sortByList = new List<string> { query.SortBy };

            var request = new BrowseRequest
            {
                UserId = query.UserId,
                OrganizationId = query.OrganizationId,
                EventId = query.EventId,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                SortBy = sortByList,
                Direction = query.Direction
            };

            var result = await _postsService.BrowsePostsAsync(request);

            // Log the serialized JSON object
            _logger.LogInformation("Fetched posts: {JsonResult}", JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MiniSpace.Services.Comments.Application.Dto;
using MiniSpace.Services.Comments.Application.Queries;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Core.Wrappers;
using MiniSpace.Services.Comments.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Queries.Handlers
{
    public class SearchCommentsHandler : IQueryHandler<SearchComments, PagedResponse<CommentDto>>
    {
        private readonly IOrganizationEventsCommentRepository _organizationEventsRepository;
        private readonly IOrganizationPostsCommentRepository _organizationPostsRepository;
        private readonly IUserEventsCommentRepository _userEventsRepository;
        private readonly IUserPostsCommentRepository _userPostsRepository;

        public SearchCommentsHandler(
            IOrganizationEventsCommentRepository organizationEventsRepository,
            IOrganizationPostsCommentRepository organizationPostsRepository,
            IUserEventsCommentRepository userEventsRepository,
            IUserPostsCommentRepository userPostsRepository)
        {
            _organizationEventsRepository = organizationEventsRepository;
            _organizationPostsRepository = organizationPostsRepository;
            _userEventsRepository = userEventsRepository;
            _userPostsRepository = userPostsRepository;
        }

        public async Task<PagedResponse<CommentDto>> HandleAsync(SearchComments query, CancellationToken cancellationToken)
{
    try
    {
        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        // Initialize the list to hold all comments from all repositories
        var allComments = new List<Comment>();

        // Use the new SortByArray property
        var sortByList = query.SortByArray.ToList();

        CommentContext contextEnum;
        if (!Enum.TryParse(query.CommentContext, true, out contextEnum))
        {
            throw new ArgumentException($"Invalid CommentContext value: {query.CommentContext}");
        }

        var browseRequest = new BrowseCommentsRequest(
            pageNumber: query.Page,
            pageSize: query.Size,
            contextId: query.ContextId,
            context: contextEnum,
            parentId: query.ParentId ?? Guid.Empty,
            sortBy: sortByList,
            sortDirection: query.Direction
        );

        // Logging the browseRequest for debug purposes
        Console.WriteLine($"Searching with ContextId: {query.ContextId}, CommentContext: {query.CommentContext}");

        // Search in OrganizationEventsCommentRepository
        Console.WriteLine("Searching in OrganizationEventsCommentRepository...");
        var orgEventsComments = await _organizationEventsRepository.BrowseCommentsAsync(browseRequest);
        if (orgEventsComments?.Items != null && orgEventsComments.Items.Any())
        {
            Console.WriteLine($"Found {orgEventsComments.Items.Count()} comments in OrganizationEventsCommentRepository.");
            allComments.AddRange(orgEventsComments.Items);
        }
        else
        {
            Console.WriteLine("No comments found in OrganizationEventsCommentRepository.");
        }

        // Search in OrganizationPostsCommentRepository
        Console.WriteLine("Searching in OrganizationPostsCommentRepository...");
        var orgPostsComments = await _organizationPostsRepository.BrowseCommentsAsync(browseRequest);
        if (orgPostsComments?.Items != null && orgPostsComments.Items.Any())
        {
            Console.WriteLine($"Found {orgPostsComments.Items.Count()} comments in OrganizationPostsCommentRepository.");
            allComments.AddRange(orgPostsComments.Items);
        }
        else
        {
            Console.WriteLine("No comments found in OrganizationPostsCommentRepository.");
        }

        // Search in UserEventsCommentRepository
        Console.WriteLine("Searching in UserEventsCommentRepository...");
        var userEventsComments = await _userEventsRepository.BrowseCommentsAsync(browseRequest);
        if (userEventsComments?.Items != null && userEventsComments.Items.Any())
        {
            Console.WriteLine($"Found {userEventsComments.Items.Count()} comments in UserEventsCommentRepository.");
            allComments.AddRange(userEventsComments.Items);
        }
        else
        {
            Console.WriteLine("No comments found in UserEventsCommentRepository.");
        }

        // Search in UserPostsCommentRepository
        Console.WriteLine("Searching in UserPostsCommentRepository...");
        var userPostsComments = await _userPostsRepository.BrowseCommentsAsync(browseRequest);
        if (userPostsComments?.Items != null && userPostsComments.Items.Any())
        {
            Console.WriteLine($"Found {userPostsComments.Items.Count()} comments in UserPostsCommentRepository.");
            allComments.AddRange(userPostsComments.Items);
        }
        else
        {
            Console.WriteLine("No comments found in UserPostsCommentRepository.");
        }

        if (!allComments.Any())
        {
            Console.WriteLine("No comments found in any repository.");
        }

        // Sort and paginate the aggregated comments
        var sortedComments = sortByList.Contains("CreatedAt")
            ? allComments.OrderByDescending(c => c.CreatedAt).ToList()
            : allComments.OrderBy(c => c.Id).ToList(); // default sorting

        var pagedComments = sortedComments
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .ToList();

        var totalItems = allComments.Count;
        var commentDtos = pagedComments.Select(c => c.AsDto()).ToList();

        var response = new PagedResponse<CommentDto>(commentDtos, query.Page, query.Size, totalItems);

        // Log the response to the console
        Console.WriteLine($"Response: {System.Text.Json.JsonSerializer.Serialize(response)}");

        return response;
    }
    catch (Exception ex)
    {
        // Log exception with more context
        Console.Error.WriteLine($"Error in SearchCommentsHandler: {ex}");
        throw;
    }
}

    }
}

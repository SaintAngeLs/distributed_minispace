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
            var allComments = new List<Comment>();

            CommentContext contextEnum;
            if (!Enum.TryParse(query.CommentContext, true, out contextEnum))
            {
                throw new ArgumentException($"Invalid CommentContext value: {query.CommentContext}");
            }

            var browseRequest = new BrowseCommentsRequest(
                pageNumber: query.Pageable.Page,
                pageSize: query.Pageable.Size,
                contextId: query.ContextId,
                context: contextEnum,   
                parentId: query.ParentId ?? Guid.Empty,
                sortBy: query.Pageable.Sort.SortBy,
                sortDirection: query.Pageable.Sort.Direction
            );


            var orgEventsComments = await _organizationEventsRepository.BrowseCommentsAsync(browseRequest);
            allComments.AddRange(orgEventsComments.Items);

            var orgPostsComments = await _organizationPostsRepository.BrowseCommentsAsync(browseRequest);
            allComments.AddRange(orgPostsComments.Items);

            var userEventsComments = await _userEventsRepository.BrowseCommentsAsync(browseRequest);
            allComments.AddRange(userEventsComments.Items);

            var userPostsComments = await _userPostsRepository.BrowseCommentsAsync(browseRequest);
            allComments.AddRange(userPostsComments.Items);

            var sortedComments = allComments
                .OrderByDescending(c => c.CreatedAt)
                .Skip((query.Pageable.Page - 1) * query.Pageable.Size)
                .Take(query.Pageable.Size)
                .ToList();

            var totalItems = allComments.Count;
            var commentDtos = sortedComments.Select(c => c.AsDto()).ToList();

            return new PagedResponse<CommentDto>(commentDtos, query.Pageable.Page, query.Pageable.Size, totalItems);
        }
    }
}

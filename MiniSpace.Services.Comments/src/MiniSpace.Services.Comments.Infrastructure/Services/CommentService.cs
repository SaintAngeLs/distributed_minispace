using MiniSpace.Services.Comments.Application;
using MiniSpace.Services.Comments.Application.Commands;
using MiniSpace.Services.Comments.Application.Dto;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Application.Wrappers;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Comments.Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IAppContext _appContext;

        public CommentService(ICommentRepository commentRepository, IAppContext appContext)
        {
            _commentRepository = commentRepository;
            _appContext = appContext;
        }

        public async Task<PagedResponse<IEnumerable<CommentDto>>> BrowseCommentsAsync(SearchComments command)
        {
            if (!Enum.TryParse<CommentContext>(command.CommentContext, true, out var context))
            {
                throw new InvalidCommentContextException(command.CommentContext);
            }

            var pageNumber = command.Pageable.Page < 1 ? 1 : command.Pageable.Page;
            var pageSize = command.Pageable.Size > 10 ? 10 : command.Pageable.Size;

            var result = await _commentRepository.BrowseCommentsAsync(
                pageNumber, pageSize, command.ContextId, context, command.ParentId,
                command.Pageable.Sort.SortBy, command.Pageable.Sort.Direction);

            var identity = _appContext.Identity;
            var pagedEvents = new PagedResponse<IEnumerable<CommentDto>>(
                result.comments.Select(c => new CommentDto(c, identity.Id)),
                result.pageNumber, result.pageSize, result.totalPages, result.totalElements);

            return pagedEvents;
        }
    }
}
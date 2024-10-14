using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Reactions.Application.Dto;
using MiniSpace.Services.Reactions.Application.Queries;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Extensions;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Queries.Handlers
{
    public class GetReactionsHandler : IQueryHandler<GetReactions, IEnumerable<ReactionDto>>
    {
        private readonly IReactionsUserPostRepository _userPostRepository;
        private readonly IReactionsOrganizationsPostRepository _orgPostRepository;
        private readonly IReactionsUserEventRepository _userEventRepository;
        private readonly IReactionsOrganizationsEventRepository _orgEventRepository;
        private readonly IReactionsUserPostCommentsRepository _userPostCommentsRepository;
        private readonly IReactionsOrganizationsPostCommentsRepository _orgPostCommentsRepository;
        private readonly IReactionsUserEventCommentsRepository _userEventCommentsRepository;
        private readonly IReactionsOrganizationsEventCommentsRepository _orgEventCommentsRepository;

        public GetReactionsHandler(
            IReactionsUserPostRepository userPostRepository,
            IReactionsOrganizationsPostRepository orgPostRepository,
            IReactionsUserEventRepository userEventRepository,
            IReactionsOrganizationsEventRepository orgEventRepository,
            IReactionsUserPostCommentsRepository userPostCommentsRepository,
            IReactionsOrganizationsPostCommentsRepository orgPostCommentsRepository,
            IReactionsUserEventCommentsRepository userEventCommentsRepository,
            IReactionsOrganizationsEventCommentsRepository orgEventCommentsRepository)
        {
            _userPostRepository = userPostRepository;
            _orgPostRepository = orgPostRepository;
            _userEventRepository = userEventRepository;
            _orgEventRepository = orgEventRepository;
            _userPostCommentsRepository = userPostCommentsRepository;
            _orgPostCommentsRepository = orgPostCommentsRepository;
            _userEventCommentsRepository = userEventCommentsRepository;
            _orgEventCommentsRepository = orgEventCommentsRepository;
        }

        public async Task<IEnumerable<ReactionDto>> HandleAsync(GetReactions query, CancellationToken cancellationToken)
        {
            List<Reaction> reactions = new();

            switch (query.ContentType)
            {
                case ReactionContentType.Post:
                    var userPostReactions = await _userPostRepository.GetByContentIdAsync(query.ContentId);
                    var orgPostReactions = await _orgPostRepository.GetByContentIdAsync(query.ContentId);

                    reactions.AddRange(userPostReactions);
                    reactions.AddRange(orgPostReactions);
                    break;

                case ReactionContentType.Event:
                    var userEventReactions = await _userEventRepository.GetByContentIdAsync(query.ContentId);
                    var orgEventReactions = await _orgEventRepository.GetByContentIdAsync(query.ContentId);

                    reactions.AddRange(userEventReactions);
                    reactions.AddRange(orgEventReactions);
                    break;

                case ReactionContentType.Comment:
                    var userPostCommentReactions = await _userPostCommentsRepository.GetByContentIdAsync(query.ContentId);
                    var orgPostCommentReactions = await _orgPostCommentsRepository.GetByContentIdAsync(query.ContentId);
                    var userEventCommentReactions = await _userEventCommentsRepository.GetByContentIdAsync(query.ContentId);
                    var orgEventCommentReactions = await _orgEventCommentsRepository.GetByContentIdAsync(query.ContentId);

                    reactions.AddRange(userPostCommentReactions);
                    reactions.AddRange(orgPostCommentReactions);
                    reactions.AddRange(userEventCommentReactions);
                    reactions.AddRange(orgEventCommentReactions);
                    break;
            }

            return reactions.Select(r => r.AsDto());
        }
    }
}

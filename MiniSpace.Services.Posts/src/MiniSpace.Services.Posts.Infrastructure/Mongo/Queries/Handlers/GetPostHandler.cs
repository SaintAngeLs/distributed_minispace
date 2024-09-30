using System.Diagnostics.CodeAnalysis;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.Queries;
using MiniSpace.Services.Posts.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetPostHandler : IQueryHandler<GetPost, PostDto>
    {
        private readonly IOrganizationEventPostRepository _organizationEventPostRepository;
        private readonly IOrganizationPostRepository _organizationPostRepository;
        private readonly IUserEventPostRepository _userEventPostRepository;
        private readonly IUserPostRepository _userPostRepository;

        public GetPostHandler(
            IOrganizationEventPostRepository organizationEventPostRepository,
            IOrganizationPostRepository organizationPostRepository,
            IUserEventPostRepository userEventPostRepository,
            IUserPostRepository userPostRepository)
        {
            _organizationEventPostRepository = organizationEventPostRepository;
            _organizationPostRepository = organizationPostRepository;
            _userEventPostRepository = userEventPostRepository;
            _userPostRepository = userPostRepository;
        }

        public async Task<PostDto> HandleAsync(GetPost query, CancellationToken cancellationToken)
        {
            Post post = null;

            post = await _organizationEventPostRepository.GetAsync(query.PostId);
            if (post != null)
            {
                return post.AsDto();
            }

            post = await _organizationPostRepository.GetAsync(query.PostId);
            if (post != null)
            {
                return post.AsDto();
            }

            post = await _userEventPostRepository.GetAsync(query.PostId);
            if (post != null)
            {
                return post.AsDto();
            }

            post = await _userPostRepository.GetAsync(query.PostId);
            if (post != null)
            {
                return post.AsDto();
            }

            return null;
        }
    }
}

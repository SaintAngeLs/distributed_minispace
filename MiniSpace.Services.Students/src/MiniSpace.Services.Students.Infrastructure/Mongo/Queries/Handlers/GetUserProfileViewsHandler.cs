using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Core.Wrappers;

namespace MiniSpace.Services.Students.Application.Queries.Handlers
{
    public class GetUserProfileViewsHandler : IQueryHandler<GetUserProfileViews, PagedResponse<UserProfileViewDto>>
    {
        private readonly IUserProfileViewsForUserRepository _userProfileViewsRepository;

        public GetUserProfileViewsHandler(IUserProfileViewsForUserRepository userProfileViewsRepository)
        {
            _userProfileViewsRepository = userProfileViewsRepository;
        }

        public async Task<PagedResponse<UserProfileViewDto>> HandleAsync(GetUserProfileViews query, CancellationToken cancellationToken = default)
        {
            var userProfileViews = await _userProfileViewsRepository.GetAsync(query.UserId);

            if (userProfileViews == null || !userProfileViews.Views.Any())
            {
                return new PagedResponse<UserProfileViewDto>(
                    Enumerable.Empty<UserProfileViewDto>(), 
                    query.PageNumber, 
                    query.PageSize, 
                    0);
            }

            var totalItems = userProfileViews.Views.Count();
            var totalPages = (int)System.Math.Ceiling(totalItems / (double)query.PageSize);
            
            var views = userProfileViews.Views
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(view => new UserProfileViewDto
                {
                    UserProfileId = view.UserProfileId,
                    Date = view.Date,
                    IpAddress = view.IpAddress,
                    DeviceType = view.DeviceType,
                    OperatingSystem = view.OperatingSystem
                });

            var pagedResponse = new PagedResponse<UserProfileViewDto>(
                views,
                query.PageNumber,
                query.PageSize,
                totalItems);

            return pagedResponse;
        }
    }
}

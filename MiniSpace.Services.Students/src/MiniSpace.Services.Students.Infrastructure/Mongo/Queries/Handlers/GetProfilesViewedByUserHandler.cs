using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Core.Wrappers;

namespace MiniSpace.Services.Students.Application.Queries.Handlers
{
    public class GetProfilesViewedByUserHandler : IQueryHandler<GetProfilesViewedByUser, PagedResponse<UserProfileViewDto>>
    {
        private readonly IUserViewingProfilesRepository _userViewingProfilesRepository;

        public GetProfilesViewedByUserHandler(IUserViewingProfilesRepository userViewingProfilesRepository)
        {
            _userViewingProfilesRepository = userViewingProfilesRepository;
        }

        public async Task<PagedResponse<UserProfileViewDto>> HandleAsync(GetProfilesViewedByUser query, CancellationToken cancellationToken)
        {
            var userViewingProfiles = await _userViewingProfilesRepository.GetAsync(query.UserId);

            if (userViewingProfiles == null || !userViewingProfiles.ViewedProfiles.Any())
            {
                return new PagedResponse<UserProfileViewDto>(Enumerable.Empty<UserProfileViewDto>(), query.PageNumber, query.PageSize, 0);
            }

            var totalItems = userViewingProfiles.ViewedProfiles.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);

            var pagedViews = userViewingProfiles.ViewedProfiles
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(view => new UserProfileViewDto
                {
                    UserProfileId = view.UserProfileId,
                    Date = view.Date,
                    IpAddress = view.IpAddress,
                    DeviceType = view.DeviceType,
                    OperatingSystem = view.OperatingSystem
                })
                .ToList();

            return new PagedResponse<UserProfileViewDto>(pagedViews, query.PageNumber, query.PageSize, totalItems);
        }
    }
}

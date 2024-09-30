using System;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Core.Wrappers;

namespace MiniSpace.Services.Students.Application.Queries
{
    public class GetProfilesViewedByUser : IQuery<PagedResponse<UserProfileViewDto>>
    {
        public Guid UserId { get; }
        public int PageNumber { get; }
        public int PageSize { get; }

        public GetProfilesViewedByUser(Guid userId, int pageNumber = 1, int pageSize = 10)
        {
            UserId = userId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}

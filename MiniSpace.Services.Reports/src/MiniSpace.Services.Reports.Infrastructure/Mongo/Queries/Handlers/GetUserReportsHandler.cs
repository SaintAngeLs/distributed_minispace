using Paralax.CQRS.Queries;
using MiniSpace.Services.Reports.Application;
using MiniSpace.Services.Reports.Application.DTO;
using MiniSpace.Services.Reports.Application.Queries;
using MiniSpace.Services.Reports.Core.Repositories;
using MiniSpace.Services.Reports.Core.Wrappers;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Queries.Handlers
{
    public class GetUserReportsHandler : IQueryHandler<GetUserReports, PagedResponse<ReportDto>>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IAppContext _appContext;

        public GetUserReportsHandler(IReportRepository reportRepository, IAppContext appContext)
        {
            _reportRepository = reportRepository;
            _appContext = appContext;
        }

        public async Task<PagedResponse<ReportDto>> HandleAsync(GetUserReports query, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != query.UserId)
            {
                return new PagedResponse<ReportDto>(Enumerable.Empty<ReportDto>(),
                    query.Page, query.Results, 0);
            }

            var result = await _reportRepository.BrowseUserReportsAsync(
                query.Page, query.Results, query.UserId, query.SortBy, query.Direction);

            var reports = result.reports.Select(r => new ReportDto(r));
            return new PagedResponse<ReportDto>(reports, result.pageNumber, result.pageSize, result.totalElements);
        }
    }
}

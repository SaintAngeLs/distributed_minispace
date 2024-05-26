using Convey.CQRS.Queries;
using MiniSpace.Services.Reports.Application;
using MiniSpace.Services.Reports.Application.DTO;
using MiniSpace.Services.Reports.Application.Queries;
using MiniSpace.Services.Reports.Core.Repositories;
using MiniSpace.Services.Reports.Core.Wrappers;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Queries.Handlers
{
    public class GetStudentReportsHandler : IQueryHandler<GetStudentReports, PagedResponse<IEnumerable<ReportDto>>>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IAppContext _appContext;

        public GetStudentReportsHandler(IReportRepository reportRepository, IAppContext appContext)
        {
            _reportRepository = reportRepository;
            _appContext = appContext;
        }

        public async Task<PagedResponse<IEnumerable<ReportDto>>> HandleAsync(GetStudentReports query, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != query.StudentId)
            {
                return new PagedResponse<IEnumerable<ReportDto>>(Enumerable.Empty<ReportDto>(),
                    1, query.Results, 0, 0);
            }
            
            var result = await _reportRepository.BrowseStudentReportsAsync(query.Page, query.Results,
                query.StudentId, Enumerable.Empty<string>(), "dsc");
            
            return new PagedResponse<IEnumerable<ReportDto>>(result.reports.Select(r => new ReportDto(r)), 
                result.pageNumber, result.pageSize, result.totalPages, result.totalElements);
        }
    }
}
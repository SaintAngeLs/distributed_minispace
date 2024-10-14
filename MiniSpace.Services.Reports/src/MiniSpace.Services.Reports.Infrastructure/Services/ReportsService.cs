using MiniSpace.Services.Reports.Application;
using MiniSpace.Services.Reports.Application.DTO;
using MiniSpace.Services.Reports.Application.Exceptions;
using MiniSpace.Services.Reports.Application.Queries;
using MiniSpace.Services.Reports.Application.Services;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Core.Repositories;
using MiniSpace.Services.Reports.Core.Wrappers;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Reports.Infrastructure.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IReportValidator _reportValidator;
        private readonly IAppContext _appContext;

        public ReportsService(IReportRepository reportRepository, IReportValidator reportValidator, IAppContext appContext)
        {
            _reportRepository = reportRepository;
            _reportValidator = reportValidator;
            _appContext = appContext;
        }

        public async Task<PagedResponse<ReportDto>> BrowseReportsAsync(SearchReports query)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && !identity.IsAdmin)
            {
                throw new UnauthorizedReportSearchAttemptException(identity.Id, identity.Role);
            }

            var contextTypes = query.ContextTypes
                .Select(ct => _reportValidator.ParseContextType(ct))
                .ToList();

            var states = query.States
                .Select(status => _reportValidator.ParseStatus(status))
                .ToList();

            var pageNumber = query.Page < 1 ? 1 : query.Page;
            var pageSize = query.Size > 10 ? 10 : query.Size;

            var result = await _reportRepository.BrowseReportsAsync(
                pageNumber, pageSize, contextTypes, states, query.ReviewerId, query.SortBy, query.Direction);

            var pagedReports = new PagedResponse<ReportDto>(
                result.reports.Select(r => new ReportDto(r)),
                result.pageNumber, result.pageSize, result.totalElements);

            return pagedReports;
        }
    }
}

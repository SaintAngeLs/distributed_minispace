using MiniSpace.Services.Reports.Application;
using MiniSpace.Services.Reports.Application.Commands;
using MiniSpace.Services.Reports.Application.DTO;
using MiniSpace.Services.Reports.Application.Exceptions;
using MiniSpace.Services.Reports.Application.Services;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Core.Repositories;
using MiniSpace.Services.Reports.Core.Wrappers;

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

         public async Task<PagedResponse<ReportDto>> BrowseReportsAsync(SearchReports command)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && !identity.IsAdmin)
            {
                throw new UnauthorizedReportSearchAttemptException(identity.Id, identity.Role);
            }

            var contextTypes = command.ContextTypes
                .Select(ct => _reportValidator.ParseContextType(ct))
                .ToList();

            var states = command.States
                .Select(status => _reportValidator.ParseStatus(status))
                .ToList();

            var pageNumber = command.Pageable.Page < 1 ? 1 : command.Pageable.Page;
            var pageSize = command.Pageable.Size > 10 ? 10 : command.Pageable.Size;

            var result = await _reportRepository.BrowseReportsAsync(
                pageNumber, pageSize, contextTypes, states, command.ReviewerId, 
                command.Pageable.Sort.SortBy, command.Pageable.Sort.Direction);

            var pagedReports = new PagedResponse<ReportDto>(
                result.reports.Select(r => new ReportDto(r)),
                result.pageNumber, result.pageSize, result.totalElements);

            return pagedReports;
        }
    }
}
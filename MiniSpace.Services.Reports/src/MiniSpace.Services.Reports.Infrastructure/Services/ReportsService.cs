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

        public async Task<PagedResponse<IEnumerable<ReportDto>>> BrowseReportsAsync(SearchReports command)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && !identity.IsAdmin)
            {
                throw new UnauthorizedReportSearchAttemptException(identity.Id, identity.Role);
            }

            var contextTypes = new List<ContextType>();
            foreach (var contextType in command.ContextTypes)
            {
                contextTypes.Add(_reportValidator.ParseContextType(contextType));
            }
            
            var states = new List<ReportState>();
            foreach (var status in command.States)
            {
                states.Add(_reportValidator.ParseStatus(status));
            }

            var pageNumber = command.Pageable.Page < 1 ? 1 : command.Pageable.Page;
            var pageSize = command.Pageable.Size > 10 ? 10 : command.Pageable.Size;

            var result = await _reportRepository.BrowseReportsAsync(pageNumber, pageSize, 
                contextTypes, states, command.Pageable.Sort.SortBy, command.Pageable.Sort.Direction);

            var pagedReports = new PagedResponse<IEnumerable<ReportDto>>(
                result.reports.Select(r => new ReportDto(r)),
                result.pageNumber, result.pageSize, result.totalPages, result.totalElements);

            return pagedReports;
        }
    }
}
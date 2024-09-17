using MiniSpace.Services.Reports.Application.Commands;
using MiniSpace.Services.Reports.Application.DTO;
using MiniSpace.Services.Reports.Core.Wrappers;

namespace MiniSpace.Services.Reports.Application.Services
{
    public interface IReportsService
    {
        Task<PagedResponse<ReportDto>> BrowseReportsAsync(SearchReports command);
    }
}
using MiniSpace.Services.Reports.Core.Entities;

namespace MiniSpace.Services.Reports.Core.Repositories
{
    public interface IReportRepository
    {
        Task<Report> GetAsync(Guid id);
        Task<IEnumerable<Report>> GetUserActiveReportsAsync(Guid userId);
        Task AddAsync(Report report);
        Task UpdateAsync(Report report);
        Task DeleteAsync(Guid id);
        Task<(IEnumerable<Report> reports, int pageNumber,int pageSize, int totalPages, int totalElements)> BrowseReportsAsync(
            int pageNumber, int pageSize, IEnumerable<ContextType> contextTypes, IEnumerable<ReportState> states,
            Guid reviewerId, string sortBy, string direction);
        Task<(IEnumerable<Report> reports, int pageNumber,int pageSize, int totalPages, int totalElements)> BrowseUserReportsAsync(
            int pageNumber, int pageSize, Guid userId, string sortBy, string direction);
    }
}
using MiniSpace.Services.Reports.Core.Entities;

namespace MiniSpace.Services.Reports.Core.Repositories
{
    public interface IReportRepository
    {
        Task<Report> GetAsync(Guid id);
        Task<IEnumerable<Report>> GetStudentActiveReportsAsync(Guid studentId);
        Task AddAsync(Report report);
        Task UpdateAsync(Report report);
        Task DeleteAsync(Guid id);
        Task<(IEnumerable<Report> reports, int pageNumber,int pageSize, int totalPages, int totalElements)> BrowseReportsAsync(
            int pageNumber, int pageSize, IEnumerable<ContextType> contextTypes, IEnumerable<ReportState> states,
            IEnumerable<string> sortBy, string direction);
        Task<(IEnumerable<Report> reports, int pageNumber,int pageSize, int totalPages, int totalElements)> BrowseStudentReportsAsync(
            int pageNumber, int pageSize, Guid studentId, IEnumerable<string> sortBy, string direction);
    }
}
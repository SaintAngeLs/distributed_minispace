using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Reports
{
    public interface IReportsService
    {
        Task<HttpResponse<PagedResponseDto<IEnumerable<ReportDto>>>> SearchReportsAsync(
            IEnumerable<string> contextTypes, IEnumerable<string> states, Guid reviewerId, PageableDto pageable);
        
        Task<HttpResponse<object>> CreateReportAsync(Guid reportId, Guid issuerId, Guid targetId, Guid targetOwnerId,
            string contextType, string category, string reason);
        
        Task DeleteReportAsync(Guid reportId);
        
        Task<HttpResponse<object>> CancelReportAsync(Guid reportId);
        
        Task<HttpResponse<object>> StartReportReviewAsync(Guid reportId, Guid reviewerId);
        
        Task<HttpResponse<object>> ResolveReportAsync(Guid reportId);
        
        Task<HttpResponse<object>> RejectReportAsync(Guid reportId);

        Task<HttpResponse<PagedResponseDto<IEnumerable<ReportDto>>>> GetStudentReports(Guid studentId, int page,
            int results);
    }
}
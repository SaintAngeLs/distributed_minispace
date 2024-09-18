using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.DTO.Wrappers;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Reports
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

        Task<PagedResponseDto<IEnumerable<ReportDto>>> GetStudentReportsAsync(Guid studentId,
            int page, int results);
    }
}
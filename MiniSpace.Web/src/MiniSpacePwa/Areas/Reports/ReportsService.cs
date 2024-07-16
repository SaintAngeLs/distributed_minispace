using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpacePwa.Areas.Identity;
using MiniSpacePwa.Data.Reports;
using MiniSpacePwa.DTO;
using MiniSpacePwa.DTO.Wrappers;
using MiniSpacePwa.HttpClients;

namespace MiniSpacePwa.Areas.Reports
{
    public class ReportsService: IReportsService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;
        
        public ReportsService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }
        
        public Task<HttpResponse<PagedResponseDto<IEnumerable<ReportDto>>>> SearchReportsAsync(
            IEnumerable<string> contextTypes, IEnumerable<string> states, Guid reviewerId, PageableDto pageable)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<SearchReports, PagedResponseDto<IEnumerable<ReportDto>>>("reports/search", 
                new (contextTypes, states, reviewerId, pageable));
        }
        
        public Task<HttpResponse<object>> CreateReportAsync(Guid reportId, Guid issuerId, Guid targetId, Guid targetOwnerId,
            string contextType, string category, string reason)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>("reports",
                new { reportId, issuerId, targetId, targetOwnerId, contextType, category, reason });
        }
        
        public Task DeleteReportAsync(Guid reportId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"reports/{reportId}");
        }
        
        public Task<HttpResponse<object>> CancelReportAsync(Guid reportId)
        {
            
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>($"reports/{reportId}/cancel", new { reportId});
        }
        
        public Task<HttpResponse<object>> StartReportReviewAsync(Guid reportId, Guid reviewerId)
        {
            
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>($"reports/{reportId}/start-review", 
                new { reportId, reviewerId });
        }
        
        public Task<HttpResponse<object>> ResolveReportAsync(Guid reportId)
        {
            
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>($"reports/{reportId}/resolve", new { reportId});
        }
        
        public Task<HttpResponse<object>> RejectReportAsync(Guid reportId)
        {
            
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>($"reports/{reportId}/reject", new { reportId});
        }
        
        public Task<PagedResponseDto<IEnumerable<ReportDto>>> GetStudentReportsAsync(Guid studentId,
            int page, int results)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<PagedResponseDto<IEnumerable<ReportDto>>>(
                $"reports/students/{studentId}?page={page}&results={results}");
        }   
    }
}

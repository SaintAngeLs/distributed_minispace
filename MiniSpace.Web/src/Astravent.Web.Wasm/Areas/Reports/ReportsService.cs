using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.Areas.Identity;
using Astravent.Web.Wasm.Areas.Reports.CommandsDto;
using Astravent.Web.Wasm.Data.Reports;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.DTO.Wrappers;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Reports
{
    public class ReportsService : IReportsService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;

        public ReportsService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }

        public async Task<HttpResponse<PagedResponseDto<IEnumerable<ReportDto>>>> SearchReportsAsync(
            IEnumerable<string> contextTypes, IEnumerable<string> states, Guid reviewerId, PageableDto pageable)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            var queryParams = new List<string>();

            if (contextTypes != null)
            {
                foreach (var contextType in contextTypes)
                {
                    queryParams.Add($"contextTypes={contextType}");
                }
            }

            if (states != null)
            {
                foreach (var state in states)
                {
                    queryParams.Add($"states={state}");
                }
            }

            if (reviewerId != Guid.Empty)
            {
                queryParams.Add($"reviewerId={reviewerId}");
            }

            if (pageable != null)
            {
                queryParams.Add($"page={pageable.Page}");
                queryParams.Add($"size={pageable.Size}");
            }

            var queryString = string.Join("&", queryParams);

            return await _httpClient.GetAsync<HttpResponse<PagedResponseDto<IEnumerable<ReportDto>>>>(
                $"reports/search?{queryString}");
        }

        public async Task<HttpResponse<object>> CreateReportAsync(CreateReportCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<object, object>("reports", command);
        }

        public async Task DeleteReportAsync(Guid reportId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.DeleteAsync($"reports/{reportId}");
        }

        public async Task<HttpResponse<object>> CancelReportAsync(Guid reportId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<object, object>($"reports/{reportId}/cancel", new { reportId });
        }

        public async Task<HttpResponse<object>> StartReportReviewAsync(Guid reportId, Guid reviewerId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<object, object>($"reports/{reportId}/start-review",
                new { reportId, reviewerId });
        }

        public async Task<HttpResponse<object>> ResolveReportAsync(Guid reportId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<object, object>($"reports/{reportId}/resolve", new { reportId });
        }

        public async Task<HttpResponse<object>> RejectReportAsync(Guid reportId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<object, object>($"reports/{reportId}/reject", new { reportId });
        }

        public async Task<HttpResponse<PagedResponseDto<IEnumerable<ReportDto>>>> GetStudentReportsAsync(Guid studentId,
            int page, int results)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<HttpResponse<PagedResponseDto<IEnumerable<ReportDto>>>>(
                $"reports/students/{studentId}?page={page}&results={results}");
        }
    }
}

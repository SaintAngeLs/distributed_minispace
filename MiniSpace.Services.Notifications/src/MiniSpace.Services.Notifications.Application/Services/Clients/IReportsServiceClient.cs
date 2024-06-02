using System;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Services.Clients
{
    public interface IReportsServiceClient
    {
        Task<ReportDto> GetReportAsync(Guid reportId);
        Task<IEnumerable<ReportDto>> GetReportsByUserIdAsync(Guid userId);
    }
}
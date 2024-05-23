using MiniSpace.Services.Reports.Core.Entities;

namespace MiniSpace.Services.Reports.Application.Services
{
    public interface IReportValidator
    {
        ContextType ParseContextType(string contextType);
        ReportCategory ParseCategory(string category);
        void ValidateActiveReports(int activeReports);
        void ValidateReason(string reason);
    }
}
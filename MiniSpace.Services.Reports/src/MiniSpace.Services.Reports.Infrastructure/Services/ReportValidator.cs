using MiniSpace.Services.Reports.Application.Exceptions;
using MiniSpace.Services.Reports.Application.Services;
using MiniSpace.Services.Reports.Core.Entities;

namespace MiniSpace.Services.Reports.Infrastructure.Services
{
    public class ReportValidator : IReportValidator
    {
        private const int MaxActiveReports = 3;
        private const int MaxReasonLength = 1000;
        
        public ContextType ParseContextType(string contextType)
        {
            if (!Enum.TryParse<ContextType>(contextType, true, out var parsedContextType))
            {
                throw new InvalidContextTypeException(contextType);
            }

            return parsedContextType;
        }
        
        public ReportCategory ParseCategory(string category)
        {
            if (!Enum.TryParse<ReportCategory>(category, true, out var parsedCategory))
            {
                throw new InvalidReportCategoryException(category);
            }

            return parsedCategory;
        }
        
        public void ValidateActiveReports(int activeReports)
        {
            if (activeReports >= MaxActiveReports)
            {
                throw new StudentTooManyActiveReportsException(activeReports, MaxActiveReports);
            }
        }
        
        public void ValidateReason(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason) || reason.Length > MaxReasonLength)
            {
                throw new InvalidReasonException(reason);
            }
        }
        
        
    }
}
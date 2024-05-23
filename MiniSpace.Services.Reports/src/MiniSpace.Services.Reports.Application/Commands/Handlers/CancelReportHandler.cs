using Convey.CQRS.Commands;
using MiniSpace.Services.Reports.Application.Exceptions;
using MiniSpace.Services.Reports.Core.Repositories;

namespace MiniSpace.Services.Reports.Application.Commands.Handlers
{
    public class CancelReportHandler : ICommandHandler<CancelReport>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IAppContext _appContext;

        public CancelReportHandler(IReportRepository reportRepository, IAppContext appContext)
        {
            _reportRepository = reportRepository;
            _appContext = appContext;
        }
        public async Task HandleAsync(CancelReport command, CancellationToken cancellationToken)
        {
            var report = await _reportRepository.GetAsync(command.ReportId);
            if (report is null)
            {
                throw new ReportNotFoundException(command.ReportId);
            }
            
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != report.IssuerId)
            {
                throw new UnauthorizedReportAccessAttemptException(report.Id, identity.Id);
            }

            report.Cancel();
            await _reportRepository.UpdateAsync(report);
        }
    }
}
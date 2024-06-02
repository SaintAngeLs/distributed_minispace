using Convey.CQRS.Commands;
using MiniSpace.Services.Reports.Application.Events;
using MiniSpace.Services.Reports.Application.Exceptions;
using MiniSpace.Services.Reports.Application.Services;
using MiniSpace.Services.Reports.Core.Repositories;

namespace MiniSpace.Services.Reports.Application.Commands.Handlers
{
    public class DeleteReportHandler : ICommandHandler<DeleteReport>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public DeleteReportHandler(IReportRepository reportRepository, IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _reportRepository = reportRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(DeleteReport command, CancellationToken cancellationToken)
        {
            var report = await _reportRepository.GetAsync(command.ReportId);
            if (report is null)
            {
                throw new ReportNotFoundException(command.ReportId);
            }
            
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && !identity.IsAdmin)
            {
                throw new UnauthorizedReportAccessAttemptException(report.Id, identity.Id);
            }

            await _reportRepository.DeleteAsync(report.Id);
            await _messageBroker.PublishAsync(new ReportDeleted(report.Id,
                report.IssuerId,
                report.TargetId,
                report.TargetOwnerId,
                report.ContextType.ToString(),
                report.Category.ToString(),
                report.Reason,
                report.State.ToString(),
                report.CreatedAt,
                report.UpdatedAt,
                report.ReviewerId
            ));
        }
    }
}
using Paralax.CQRS.Commands;
using MiniSpace.Services.Reports.Application.Events;
using MiniSpace.Services.Reports.Application.Exceptions;
using MiniSpace.Services.Reports.Application.Services;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Core.Repositories;

namespace MiniSpace.Services.Reports.Application.Commands.Handlers
{
    public class CreateReportHandler : ICommandHandler<CreateReport>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IReportValidator _reportValidator;
        private readonly IAppContext _appContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMessageBroker _messageBroker;
        
        public CreateReportHandler(IReportRepository reportRepository, IReportValidator reportValidator, 
            IAppContext appContext, IDateTimeProvider dateTimeProvider, IMessageBroker messageBroker)
        {
            _reportRepository = reportRepository;
            _reportValidator = reportValidator;
            _appContext = appContext;
            _dateTimeProvider = dateTimeProvider;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(CreateReport command, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != command.IssuerId)
            {
                throw new UnauthorizedReportCreationAttemptException(identity.Id, command.IssuerId);
            }
            
            var contextType = _reportValidator.ParseContextType(command.ContextType);
            var category = _reportValidator.ParseCategory(command.Category);
            _reportValidator.ValidateReason(command.Reason);
            var activeStudentReports = await _reportRepository.GetUserActiveReportsAsync(command.IssuerId);
            _reportValidator.ValidateActiveReports(activeStudentReports.Count());

            var report = Report.Create(command.ReportId, command.IssuerId, command.TargetId, command.TargetOwnerId,
                contextType, category, command.Reason, _dateTimeProvider.Now);
            await _reportRepository.AddAsync(report);
            await _messageBroker.PublishAsync(new ReportCreated(
                report.Id,
                report.IssuerId,
                report.TargetId,
                report.TargetOwnerId,
                contextType.ToString(),
                category.ToString(),
                report.Reason,
                report.State.ToString(),
                report.CreatedAt,
                report.UpdatedAt,
                report.ReviewerId
            ));
        }
    }
}
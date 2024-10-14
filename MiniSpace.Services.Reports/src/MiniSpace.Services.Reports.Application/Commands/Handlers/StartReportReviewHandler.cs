﻿using Paralax.CQRS.Commands;
using MiniSpace.Services.Reports.Application.Events;
using MiniSpace.Services.Reports.Application.Exceptions;
using MiniSpace.Services.Reports.Application.Services;
using MiniSpace.Services.Reports.Core.Repositories;

namespace MiniSpace.Services.Reports.Application.Commands.Handlers
{
    public class StartReportReviewHandler : ICommandHandler<StartReportReview>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IAppContext _appContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMessageBroker _messageBroker;
        
        public StartReportReviewHandler(IReportRepository reportRepository, IAppContext appContext,
            IDateTimeProvider dateTimeProvider, IMessageBroker messageBroker)
        {
            _reportRepository = reportRepository;
            _appContext = appContext;
            _dateTimeProvider = dateTimeProvider;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(StartReportReview command, CancellationToken cancellationToken)
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

            report.StartReview(command.ReviewerId, _dateTimeProvider.Now);
            await _reportRepository.UpdateAsync(report);
            await _messageBroker.PublishAsync(new ReportReviewStarted(report.Id,
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
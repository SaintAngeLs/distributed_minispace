using Convey.Logging.CQRS;
using MiniSpace.Services.Reports.Application.Commands;
using MiniSpace.Services.Reports.Application.Events.External;

namespace MiniSpace.Services.Reports.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(CreateReport),  new HandlerLogTemplate
                    {
                        After = "Created a new report with ID: {ReportId}."
                    }
                },
                {
                    typeof(CancelReport),  new HandlerLogTemplate
                    {
                        After = "Canceled a report with ID: {ReportId}."
                    }
                },
                {
                    typeof(StartReportReview),  new HandlerLogTemplate
                    {
                        After = "Started a review for a report with ID: {ReportId}."
                    }
                },
                {
                    typeof(ResolveReport),  new HandlerLogTemplate
                    {
                        After = "Resolved a report with ID: {ReportId}."
                    }
                },
                {
                    typeof(RejectReport),  new HandlerLogTemplate
                    {
                        After = "Rejected a report with ID: {ReportId}."
                    }
                },
            };
        
        public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var key = message.GetType();
            return MessageTemplates.TryGetValue(key, out var template) ? template : null;
        }
    }
}

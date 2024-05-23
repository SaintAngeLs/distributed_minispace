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
            };
        
        public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var key = message.GetType();
            return MessageTemplates.TryGetValue(key, out var template) ? template : null;
        }
    }
}

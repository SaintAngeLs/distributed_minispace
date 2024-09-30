using Paralax.CQRS.Logging;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Communication.Application.Commands;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Communication.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private readonly ILogger<MessageToLogTemplateMapper> _logger;

        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates => new Dictionary<Type, HandlerLogTemplate>
        {
            {
                typeof(AddUserToChat), new HandlerLogTemplate
                {
                    After = "Added user with id: {UserId} to chat with id: {ChatId}."
                }
            },
            {
                typeof(CreateChat), new HandlerLogTemplate
                {
                    After = "Created chat with id: {ChatId}, participants: {ParticipantIds}, and name: '{ChatName}'."
                }
            },
            {
                typeof(DeleteChat), new HandlerLogTemplate
                {
                    After = "Deleted chat with id: {ChatId}."
                }
            },
            {
                typeof(DeleteMessage), new HandlerLogTemplate
                {
                    After = "Deleted message with id: {MessageId} from chat with id: {ChatId}."
                }
            },
            {
                typeof(RemoveUserFromChat), new HandlerLogTemplate
                {
                    After = "Removed user with id: {UserId} from chat with id: {ChatId}."
                }
            },
            {
                typeof(SendMessage), new HandlerLogTemplate
                {
                    After = "Sent message in chat with id: {ChatId} by user with id: {SenderId}, content: '{Content}', and type: '{MessageType}'."
                }
            },
            {
                typeof(UpdateMessageStatus), new HandlerLogTemplate
                {
                    After = "Updated message status to '{Status}' for message with id: {MessageId} in chat with id: {ChatId}."
                }
            }
        };

        public MessageToLogTemplateMapper(ILogger<MessageToLogTemplateMapper> logger)
        {
            _logger = logger;
        }

        public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var messageType = message.GetType();
            _logger.LogInformation($"Attempting to map message type: {messageType.Name}");
            if (MessageTemplates.TryGetValue(messageType, out var template))
            {
                _logger.LogInformation($"Mapping found. Template: {template.After}");
                return template;
            }
            _logger.LogWarning($"No mapping found for message type: {messageType.Name}");
            return null;
        }
    }
}

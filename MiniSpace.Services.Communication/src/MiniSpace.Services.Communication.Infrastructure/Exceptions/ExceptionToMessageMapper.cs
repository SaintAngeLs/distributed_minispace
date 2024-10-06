using Paralax.MessageBrokers.RabbitMQ;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Events.Rejected;
using MiniSpace.Services.Communication.Application.Exceptions;
using System;

namespace MiniSpace.Services.Communication.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
            {
                // ChatNotFoundException ex => message switch
                // {
                //     DeleteChat command => new ChatDeletionRejected(command.ChatId, "Chat not found", ex.Code),
                //     CreateChat command => new ChatCreationRejected(command.ChatId, "Chat not found", ex.Code),
                //     AddUserToChat command => new UserAdditionToChatRejected(command.ChatId, command.UserId, "Chat not found", ex.Code),
                //     SendMessage command => new MessageSendRejected(command.ChatId, Guid.NewGuid(), "Chat not found", ex.Code),
                //     _ => new ChatProcessRejected(ex.ChatId, ex.Message, ex.Code),
                // },
                // MessageNotFoundException ex => message switch
                // {
                //     DeleteMessage command => new MessageSendRejected(command.ChatId, command.MessageId, "Message not found", ex.Code),
                //     _ => new MessageProcessRejected(ex.MessageId, ex.Message, ex.Code),
                // },
                // InvalidChatOperationException ex => message switch
                // {
                //     AddUserToChat command => new UserAdditionToChatRejected(command.ChatId, command.UserId, ex.Message, ex.Code),
                //     SendMessage command => new MessageSendRejected(command.ChatId, Guid.NewGuid(), ex.Message, ex.Code),
                //     _ => new ChatProcessRejected(Guid.Empty, ex.Message, ex.Code),
                // },
                // AppException ex => message switch
                // {
                //     _ => new ChatProcessRejected(Guid.Empty, ex.Message, ex.Code)
                // },
                // _ => null
            };
    }
}

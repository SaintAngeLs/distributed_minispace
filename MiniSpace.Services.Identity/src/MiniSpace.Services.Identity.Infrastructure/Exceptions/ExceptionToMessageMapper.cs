using System;
using Convey.MessageBrokers.RabbitMQ;
using MiniSpace.Services.Identity.Application.Commands;
using MiniSpace.Services.Identity.Application.Events.Rejected;
using MiniSpace.Services.Identity.Core.Exceptions;

namespace MiniSpace.Services.Identity.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch

            {
                EmailInUseException ex => new SignUpRejected(ex.Email, ex.Message, ex.Code),
                InvalidCredentialsException ex => new SignInRejected(ex.Email, ex.Message, ex.Code),
                InvalidEmailException ex => message switch
                {
                    SignIn command => new SignInRejected(command.Email, ex.Message, ex.Code),
                    SignUpRejected command => new SignUpRejected(command.Email, ex.Message, ex.Code),
                    _ => null
                },
                _ => null
            };
    }
}
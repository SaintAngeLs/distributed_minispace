using System;
using System.Diagnostics.CodeAnalysis;
using Convey.MessageBrokers.RabbitMQ;
using MiniSpace.Services.Identity.Application.Commands;
using MiniSpace.Services.Identity.Application.Events.Rejected;
using MiniSpace.Services.Identity.Application.Exceptions;
using MiniSpace.Services.Identity.Core.Exceptions;

namespace MiniSpace.Services.Identity.Infrastructure.Exceptions
{
    [ExcludeFromCodeCoverage]
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
                    SignUp command => new SignUpRejected(command.Email, ex.Message, ex.Code),
                    VerifyEmail command => new EmailVerificationRejected(command.Email, ex.Message, ex.Code),
                    _ => null
                },
                UserNotFoundException ex => message switch
                {
                    EnableTwoFactor command => new EnableTwoFactorRejected(command.UserId, ex.Message, ex.Code),
                    DisableTwoFactor command => new DisableTwoFactorRejected(command.UserId, ex.Message, ex.Code),
                    _ => null
                },
                InvalidTokenException ex => message switch
                {
                    VerifyEmail command => new EmailVerificationRejected(command.Email, ex.Message, ex.Code),
                    _ => null
                },
                TwoFactorAlreadyEnabledException ex => new EnableTwoFactorRejected(ex.UserId, ex.Message, ex.Code),
                TwoFactorNotEnabledException ex => new DisableTwoFactorRejected(ex.UserId, ex.Message, ex.Code),
                _ => null
            };
    }
}

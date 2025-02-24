using Paralax.MessageBrokers.RabbitMQ;
using MiniSpace.Services.Students.Application.Commands;
using MiniSpace.Services.Students.Application.Events.Rejected;
using MiniSpace.Services.Students.Application.Events.External;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Core.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Infrastructure.Exceptions
{
    [ExcludeFromCodeCoverage]
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
    
            {
                CannotChangeUserStateException ex => message switch
                {
                    ChangeUserState _ => new ChangeUserStateRejected(ex.Id,
                        ex.State.ToString().ToLowerInvariant(), ex.Message, ex.Code),
                    CompleteUserRegistration _ => new CompleteUserRegistrationRejected(ex.Id, ex.Message,
                        ex.Code),
                    _ => null
                },
                CannotUpdateUserException ex => message switch
                {
                    UpdateUser command => new UpdateUserRejected(command.UserId, ex.Message, ex.Code),
                    _ => null,
                },
                InvalidUserDateOfBirthException ex => message switch
                {
                    CompleteUserRegistration _ => new CompleteUserRegistrationRejected(ex.Id, ex.Message,
                        ex.Code),
                    _ => null,
                },
                InvalidStudentDescriptionException ex => message switch
                {
                    CompleteUserRegistration _ => new CompleteUserRegistrationRejected(ex.Id, ex.Message,
                        ex.Code),
                    UpdateUser command => new UpdateUserRejected(command.UserId, ex.Message, ex.Code),
                    _ => null,
                },
                InvalidStudentFullNameException ex => message switch
                {
                    CompleteUserRegistration _ => new CompleteUserRegistrationRejected(ex.Id, ex.Message,
                        ex.Code),
                    _ => null,
                },
                InvalidStudentProfileImageException ex => message switch
                {
                    CompleteUserRegistration _ => new CompleteUserRegistrationRejected(ex.Id, ex.Message,
                        ex.Code),
                    UpdateUser command => new UpdateUserRejected(command.UserId, ex.Message, ex.Code),
                    _ => null,
                },
                InvalidRoleException ex => message switch
                {
                    SignedUp ev => new CreateUserRejected(ev.UserId, ex.Message, ex.Code),
                    _ => null,
                },
                UserAlreadyCreatedException ex => message switch
                {
                    SignedUp ev => new CreateUserRejected(ev.UserId, ex.Message, ex.Code),
                    _ => null,
                },
                UserAlreadyRegisteredException ex => message switch
                {
                    CompleteUserRegistration _ => new CompleteUserRegistrationRejected(ex.Id, ex.Message,
                        ex.Code),
                    _ => null,
                },
                UserNotFoundException ex => message switch
                {
                    CompleteUserRegistration _ => new CompleteUserRegistrationRejected(ex.Id, ex.Message,
                        ex.Code),
                    DeleteUser command => new DeleteUserRejected(command.UserId, ex.Message, ex.Code),
                    UpdateUser command => new UpdateUserRejected(command.UserId, ex.Message, ex.Code),
                    _ => null,
                },
                UserStateAlreadySetException ex => message switch
                {
                    ChangeUserState _ => new ChangeUserStateRejected(ex.Id,
                        ex.State.ToString().ToLowerInvariant(), ex.Message, ex.Code),
                    _ => null
                },
                AnauthorizedUserAccessException ex => message switch
                {
                    DeleteUser command => new DeleteUserRejected(command.UserId, ex.Message, ex.Code),
                    _ => null,
                },
                _ => null
            };
    }    
}

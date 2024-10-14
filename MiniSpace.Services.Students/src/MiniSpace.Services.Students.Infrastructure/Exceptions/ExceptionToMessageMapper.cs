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
                CannotChangeStudentStateException ex => message switch
                {
                    ChangeStudentState _ => new ChangeStudentStateRejected(ex.Id,
                        ex.State.ToString().ToLowerInvariant(), ex.Message, ex.Code),
                    CompleteStudentRegistration _ => new CompleteStudentRegistrationRejected(ex.Id, ex.Message,
                        ex.Code),
                    _ => null
                },
                CannotUpdateStudentException ex => message switch
                {
                    UpdateStudent command => new UpdateStudentRejected(command.StudentId, ex.Message, ex.Code),
                    _ => null,
                },
                InvalidStudentDateOfBirthException ex => message switch
                {
                    CompleteStudentRegistration _ => new CompleteStudentRegistrationRejected(ex.Id, ex.Message,
                        ex.Code),
                    _ => null,
                },
                InvalidStudentDescriptionException ex => message switch
                {
                    CompleteStudentRegistration _ => new CompleteStudentRegistrationRejected(ex.Id, ex.Message,
                        ex.Code),
                    UpdateStudent command => new UpdateStudentRejected(command.StudentId, ex.Message, ex.Code),
                    _ => null,
                },
                InvalidStudentFullNameException ex => message switch
                {
                    CompleteStudentRegistration _ => new CompleteStudentRegistrationRejected(ex.Id, ex.Message,
                        ex.Code),
                    _ => null,
                },
                InvalidStudentProfileImageException ex => message switch
                {
                    CompleteStudentRegistration _ => new CompleteStudentRegistrationRejected(ex.Id, ex.Message,
                        ex.Code),
                    UpdateStudent command => new UpdateStudentRejected(command.StudentId, ex.Message, ex.Code),
                    _ => null,
                },
                InvalidRoleException ex => message switch
                {
                    SignedUp ev => new CreateStudentRejected(ev.UserId, ex.Message, ex.Code),
                    _ => null,
                },
                StudentAlreadyCreatedException ex => message switch
                {
                    SignedUp ev => new CreateStudentRejected(ev.UserId, ex.Message, ex.Code),
                    _ => null,
                },
                StudentAlreadyRegisteredException ex => message switch
                {
                    CompleteStudentRegistration _ => new CompleteStudentRegistrationRejected(ex.Id, ex.Message,
                        ex.Code),
                    _ => null,
                },
                StudentNotFoundException ex => message switch
                {
                    CompleteStudentRegistration _ => new CompleteStudentRegistrationRejected(ex.Id, ex.Message,
                        ex.Code),
                    DeleteStudent command => new DeleteStudentRejected(command.StudentId, ex.Message, ex.Code),
                    UpdateStudent command => new UpdateStudentRejected(command.StudentId, ex.Message, ex.Code),
                    _ => null,
                },
                StudentStateAlreadySetException ex => message switch
                {
                    ChangeStudentState _ => new ChangeStudentStateRejected(ex.Id,
                        ex.State.ToString().ToLowerInvariant(), ex.Message, ex.Code),
                    _ => null
                },
                UnauthorizedStudentAccessException ex => message switch
                {
                    DeleteStudent command => new DeleteStudentRejected(command.StudentId, ex.Message, ex.Code),
                    _ => null,
                },
                _ => null
            };
    }    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.MessageBrokers.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using MiniSpace.Services.Students.Application.Commands;
using MiniSpace.Services.Students.Application.Events.Rejected;
using MiniSpace.Services.Students.Application.Events.External;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Core;
using MiniSpace.Services.Students.Core.Exceptions;

namespace MiniSpace.Services.Students.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
    
            {
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
                StudentNotFoundException ex => message switch
                {
                    DeleteStudent command => new DeleteStudentRejected(command.Id, ex.Message, ex.Code),
                    _ => null,
                },
                UnauthorizedStudentAccessException ex => message switch
                {
                    DeleteStudent command => new DeleteStudentRejected(command.Id, ex.Message, ex.Code),
                    _ => null,
                },
                _ => null
            };
    }    
}

﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;

namespace MiniSpace.Services.Reactions.Application.Events.External.Handlers
{
    public class StudentCreatedHandler : IEventHandler<StudentCreated>
    {
        private readonly IStudentRepository _studentRepository;

        public StudentCreatedHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        
        public async Task HandleAsync(StudentCreated @event, CancellationToken cancellationToken)
        {
            if (await _studentRepository.ExistsAsync(@event.StudentId))
            {
                throw new StudentAlreadyAddedException(@event.StudentId);
            }
            
            await _studentRepository.AddAsync(new Student(@event.StudentId));
        }
    }
}
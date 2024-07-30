using Convey.CQRS.Commands;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Events;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class UpdateStudentLanguagesAndInterestsHandler : ICommandHandler<UpdateStudentLanguagesAndInterests>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAppContext _appContext;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public UpdateStudentLanguagesAndInterestsHandler(IStudentRepository studentRepository, IAppContext appContext,
            IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _studentRepository = studentRepository;
            _appContext = appContext;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdateStudentLanguagesAndInterests command, CancellationToken cancellationToken = default)
        {
            // Log the command received
            var commandJson = JsonSerializer.Serialize(command);
            Console.WriteLine($"Received UpdateStudentLanguagesAndInterests command: {commandJson}");

            var student = await _studentRepository.GetAsync(command.StudentId);
            if (student is null)
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != student.Id && !identity.IsAdmin)
            {
                throw new UnauthorizedStudentAccessException(command.StudentId, identity.Id);
            }

            student.UpdateLanguages(command.Languages.Select(l => (Language)Enum.Parse(typeof(Language), l)));
            student.UpdateInterests(command.Interests.Select(i => (Interest)Enum.Parse(typeof(Interest), i)));

            await _studentRepository.UpdateAsync(student);

            var studentUpdatedEvent = new StudentUpdated(
                student.Id,
                student.FullName,
                student.Description,
                student.Education.Select(e => new EducationDto
                {
                    InstitutionName = e.InstitutionName,
                    Degree = e.Degree,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Description = e.Description
                }).ToList(),
                student.Work.Select(w => new WorkDto
                {
                    Company = w.Company,
                    Position = w.Position,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    Description = w.Description
                }).ToList(),
                student.Languages.Select(l => l.ToString()).ToList(),
                student.Interests.Select(i => i.ToString()).ToList(),
                student.ContactEmail,
                student.Country,
                student.City
            );

            await _messageBroker.PublishAsync(studentUpdatedEvent);
        }
    }
}

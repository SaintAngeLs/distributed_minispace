using Paralax.CQRS.Commands;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Events;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MiniSpace.Services.Students.Application.Services;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class UpdateStudentHandler : ICommandHandler<UpdateStudent>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAppContext _appContext;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public UpdateStudentHandler(IStudentRepository studentRepository, IAppContext appContext, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _studentRepository = studentRepository;
            _appContext = appContext;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdateStudent command, CancellationToken cancellationToken = default)
        {
            var commandJson = JsonSerializer.Serialize(command);
            Console.WriteLine($"Received UpdateStudent command: {commandJson}");

            var student = await _studentRepository.GetAsync(command.StudentId);
            if (student == null)
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            student.Update(command.FirstName, command.LastName, command.Description, command.EmailNotifications, command.ContactEmail, command.PhoneNumber, command.Country, command.City, command.DateOfBirth);

            student.UpdateEducation(command.Education.Select(e => new Education(
                e.OrganizationId, e.InstitutionName, e.Degree, e.StartDate, e.EndDate, e.Description)));

            student.UpdateWork(command.Work.Select(w => new Work(
                w.OrganizationId, w.Company, w.Position, w.StartDate, w.EndDate, w.Description)));

            student.UpdateLanguages(command.Languages.Select(l => (Language)Enum.Parse(typeof(Language), l)));
            student.UpdateInterests(command.Interests.Select(i => (Interest)Enum.Parse(typeof(Interest), i)));

            if (command.EnableTwoFactor)
            {
                student.EnableTwoFactorAuthentication(command.TwoFactorSecret);
            }

            if (command.DisableTwoFactor)
            {
                student.DisableTwoFactorAuthentication();
            }

            await _studentRepository.UpdateAsync(student);

            var studentUpdatedEvent = new StudentUpdated(
                student.Id,
                student.FullName,
                student.Description,
                student.Education.Select(e => new EducationDto
                {
                    OrganizationId = e.OrganizationId,
                    InstitutionName = e.InstitutionName,
                    Degree = e.Degree,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Description = e.Description
                }).ToList(),
                student.Work.Select(w => new WorkDto
                {
                    OrganizationId = w.OrganizationId,
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
                student.City,
                student.DateOfBirth
            );

            await _messageBroker.PublishAsync(studentUpdatedEvent);
        }
    }
}

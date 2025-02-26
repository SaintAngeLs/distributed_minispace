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

namespace MiniSpace.Services.Students.Application.Commands.Handlers;

public class UpdateUserHandler : ICommandHandler<UpdateUser>
{
    private readonly IUserRepository _userRepository;
    private readonly IAppContext _appContext;
    private readonly IEventMapper _eventMapper;
    private readonly IMessageBroker _messageBroker;

    public UpdateUserHandler(
        IUserRepository userRepository,
        IAppContext appContext,
        IEventMapper eventMapper,
        IMessageBroker messageBroker)
    {
        _userRepository = userRepository;
        _appContext = appContext;
        _eventMapper = eventMapper;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(UpdateUser command, CancellationToken cancellationToken = default)
    {
        var commandJson = JsonSerializer.Serialize(command);
        Console.WriteLine($"Received UpdateStudent command: {commandJson}");

        var user = await _userRepository.GetAsync(command.Id);
        if (user == null)
        {
            throw new UserNotFoundException(command.Id);
        }

        user.UpdateUserName(command.UserName);

        user.Update(command.FirstName, command.LastName, command.Description,
            command.EmailNotifications, command.ContactEmail, command.PhoneNumber,
            command.Country, command.City, command.DateOfBirth);

        user.UpdateEducation(command.Education.Select(e => new Education(
            e.OrganizationId, e.InstitutionName, e.Degree, e.StartDate, e.EndDate, e.Description)));

        user.UpdateWork(command.Work.Select(w => new Work(
            w.OrganizationId, w.Company, w.Position, w.StartDate, w.EndDate, w.Description)));

        user.UpdateLanguages(command.Languages.Select(l => (Language)Enum.Parse(typeof(Language), l)));
        user.UpdateInterests(command.Interests.Select(i => (Interest)Enum.Parse(typeof(Interest), i)));

        if (command.EnableTwoFactor)
        {
            user.EnableTwoFactorAuthentication(command.TwoFactorSecret);
        }

        if (command.DisableTwoFactor)
        {
            user.DisableTwoFactorAuthentication();
        }

        await _userRepository.UpdateAsync(user);

        var studentUpdatedEvent = new UserUpdated(
            user.Id,
            user.UserName,
            user.FullName,
            user.Description,
            user.Education.Select(e => new EducationDto
            {
                OrganizationId = e.OrganizationId,
                InstitutionName = e.InstitutionName,
                Degree = e.Degree,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Description = e.Description
            }).ToList(),
            user.Work.Select(w => new WorkDto
            {
                OrganizationId = w.OrganizationId,
                Company = w.Company,
                Position = w.Position,
                StartDate = w.StartDate,
                EndDate = w.EndDate,
                Description = w.Description
            }).ToList(),
            user.Languages.Select(l => l.ToString()).ToList(),
            user.Interests.Select(i => i.ToString()).ToList(),
            user.ContactEmail,
            user.Country,
            user.City,
            user.DateOfBirth
        );

        await _messageBroker.PublishAsync(studentUpdatedEvent);
    }
}

using Paralax.CQRS.Commands;
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

namespace MiniSpace.Services.Students.Application.Commands.Handlers;

public class UpdateUserLanguagesAndInterestsHandler : ICommandHandler<UpdateUserLanguagesAndInterests>
{
    private readonly IUserRepository _userRepository;
    private readonly IAppContext _appContext;
    private readonly IEventMapper _eventMapper;
    private readonly IMessageBroker _messageBroker;

    public UpdateUserLanguagesAndInterestsHandler(
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

    public async Task HandleAsync(UpdateUserLanguagesAndInterests command, CancellationToken cancellationToken = default)
    {
        var commandJson = JsonSerializer.Serialize(command);
        Console.WriteLine($"Received UpdateUserLanguagesAndInterests command: {commandJson}");

        var user = await _userRepository.GetAsync(command.UserId);
        if (user is null)
        {
            throw new UserNotFoundException(command.UserId);
        }

        var identity = _appContext.Identity;
        if (identity.IsAuthenticated && identity.Id != user.Id && !identity.IsAdmin)
        {
            throw new AnauthorizedUserAccessException(command.UserId, identity.Id);
        }

        user.UpdateLanguages(command.Languages.Select(l => (Language)Enum.Parse(typeof(Language), l)));
        user.UpdateInterests(command.Interests.Select(i => (Interest)Enum.Parse(typeof(Interest), i)));

        await _userRepository.UpdateAsync(user);

        var studentUpdatedEvent = new UserUpdated(
            user.Id,
            user.UserName, 
            user.FullName,
            user.Description,
            user.Education.Select(e => new EducationDto
            {
                InstitutionName = e.InstitutionName,
                Degree = e.Degree,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Description = e.Description
            }).ToList(),
            user.Work.Select(w => new WorkDto
            {
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

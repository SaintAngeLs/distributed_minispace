using Paralax.CQRS.Events;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core;
using MiniSpace.Services.Students.Core.Events;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MiniSpace.Services.Students.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);

        public IEvent Map(IDomainEvent @event)
        {
            switch (@event)
            {
                case UserRegistrationCompleted e:
                    return new Application.Events.UserCreated(e.User.Id, e.User.FullName, e.User.ProfileImageUrl);
                case UserUpdated e:
                    return new Application.Events.UserUpdated(
                        e.User.Id, 
                        e.User.FullName, 
                        e.User.UserName,
                        e.User.Description,
                        e.User.Education.Select(ed => new Application.Dto.EducationDto 
                        {
                            InstitutionName = ed.InstitutionName,
                            Degree = ed.Degree,
                            StartDate = ed.StartDate,
                            EndDate = ed.EndDate,
                            Description = ed.Description
                        }),
                        e.User.Work.Select(w => new Application.Dto.WorkDto
                        {
                            Company = w.Company,
                            Position = w.Position,
                            StartDate = w.StartDate,
                            EndDate = w.EndDate,
                            Description = w.Description
                        }),
                        e.User.Languages.Select(i => i.ToString()),
                        e.User.Interests.Select(i => i.ToString()),
                        e.User.ContactEmail,
                        e.User.Country,
                        e.User.City,
                        e.User.DateOfBirth);
                case UserStateChanged e:
                    return new Application.Events.UserStateChanged(
                        e.User.Id, 
                        e.User.FullName,
                        e.User.State.ToString().ToLowerInvariant(), 
                        e.PreviousState.ToString().ToLowerInvariant());
            }

            return null;
        }
    }
}

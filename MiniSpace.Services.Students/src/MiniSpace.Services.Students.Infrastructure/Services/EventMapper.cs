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
                case StudentRegistrationCompleted e:
                    return new Application.Events.StudentCreated(e.Student.Id, e.Student.FullName, e.Student.ProfileImageUrl);
                case StudentUpdated e:
                    return new Application.Events.StudentUpdated(
                        e.Student.Id, 
                        e.Student.FullName, 
                        e.Student.Description,
                        e.Student.Education.Select(ed => new Application.Dto.EducationDto 
                        {
                            InstitutionName = ed.InstitutionName,
                            Degree = ed.Degree,
                            StartDate = ed.StartDate,
                            EndDate = ed.EndDate,
                            Description = ed.Description
                        }),
                        e.Student.Work.Select(w => new Application.Dto.WorkDto
                        {
                            Company = w.Company,
                            Position = w.Position,
                            StartDate = w.StartDate,
                            EndDate = w.EndDate,
                            Description = w.Description
                        }),
                        e.Student.Languages.Select(i => i.ToString()),
                        e.Student.Interests.Select(i => i.ToString()),
                        e.Student.ContactEmail,
                        e.Student.Country,
                        e.Student.City,
                        e.Student.DateOfBirth);
                case StudentStateChanged e:
                    return new Application.Events.StudentStateChanged(
                        e.Student.Id, 
                        e.Student.FullName,
                        e.Student.State.ToString().ToLowerInvariant(), 
                        e.PreviousState.ToString().ToLowerInvariant());
            }

            return null;
        }
    }
}

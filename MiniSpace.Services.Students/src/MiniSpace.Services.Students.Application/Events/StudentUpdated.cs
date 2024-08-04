using Convey.CQRS.Events;
using MiniSpace.Services.Students.Application.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentUpdated : IEvent
    {
        public Guid StudentId { get; }
        public string FullName { get; }
        public string Description { get; }
        public IEnumerable<EducationDto> Education { get; }
        public IEnumerable<WorkDto> Work { get; }
        public IEnumerable<string> Languages { get; }
        public IEnumerable<string> Interests { get; }
        public string ContactEmail { get; } 
        public string Country { get; }
        public string City { get; }
        public DateTime? DateOfBirth { get; }

        public StudentUpdated(Guid studentId, string fullName, string description,
                              IEnumerable<EducationDto> education, IEnumerable<WorkDto> work,
                              IEnumerable<string> languages, IEnumerable<string> interests,
                              string contactEmail, string country, string city, DateTime? dateOfBirth)
        {
            StudentId = studentId;
            FullName = fullName;
            Description = description;
            Education = education;
            Work = work;
            Languages = languages;
            Interests = interests;
            ContactEmail = contactEmail;
            Country = country;
            City = city;
            DateOfBirth = dateOfBirth;
        }
    }
}

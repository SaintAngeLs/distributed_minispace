using Convey.CQRS.Commands;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UpdateStudent : ICommand
    {
        public Guid StudentId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string? Description { get; }
        public bool EmailNotifications { get; }
        public IEnumerable<EducationDto> Education { get; }
        public IEnumerable<WorkDto> Work { get; }
        public IEnumerable<string> Languages { get; }
        public IEnumerable<string> Interests { get; }
        public bool EnableTwoFactor { get; }
        public bool DisableTwoFactor { get; }
        public string TwoFactorSecret { get; }
        public string? ContactEmail { get; }
        public string PhoneNumber { get; }
        public string Country { get; }
        public string City { get; }

        public UpdateStudent(Guid studentId, string firstName, string lastName, string? description, bool emailNotifications,
            IEnumerable<EducationDto> education, IEnumerable<WorkDto> work, 
            IEnumerable<string> languages, IEnumerable<string> interests,
            bool enableTwoFactor, bool disableTwoFactor, string twoFactorSecret, string? contactEmail,
            string phoneNumber, string country, string city)
        {
            StudentId = studentId;
            FirstName = firstName;
            LastName = lastName;
            Description = description;
            EmailNotifications = emailNotifications;
            Education = education ?? Enumerable.Empty<EducationDto>();
            Work = work ?? Enumerable.Empty<WorkDto>();
            Languages = languages ?? Enumerable.Empty<string>();
            Interests = interests ?? Enumerable.Empty<string>();
            EnableTwoFactor = enableTwoFactor;
            DisableTwoFactor = disableTwoFactor;
            TwoFactorSecret = twoFactorSecret;
            ContactEmail = contactEmail;
            PhoneNumber = phoneNumber;
            Country = country;
            City = city;
        }
    }
}

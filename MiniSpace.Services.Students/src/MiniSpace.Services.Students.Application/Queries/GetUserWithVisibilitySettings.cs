using Paralax.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetUserWithVisibilitySettings : IQuery<StudentWithVisibilitySettingsDto>
    {
        public Guid StudentId { get; set; }

        public GetUserWithVisibilitySettings(Guid studentId)
        {
            StudentId = studentId;
        }
    }
}

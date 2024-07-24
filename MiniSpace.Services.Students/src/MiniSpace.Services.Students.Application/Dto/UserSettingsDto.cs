using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class UserSettingsDto
    {
        public Guid StudentId { get; set; }
        public AvailableSettingsDto AvailableSettings { get; set; }
    }
}

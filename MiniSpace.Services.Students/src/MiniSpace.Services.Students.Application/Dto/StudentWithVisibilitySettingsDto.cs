using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class StudentWithVisibilitySettingsDto
    {
        public UserDto User { get; set; }
        public AvailableSettingsDto VisibilitySettings { get; set; }
    }
}

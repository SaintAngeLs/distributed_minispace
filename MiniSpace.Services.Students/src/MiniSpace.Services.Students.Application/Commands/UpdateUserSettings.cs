using Convey.CQRS.Commands;
using MiniSpace.Services.Students.Application.Dto;
using System;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UpdateUserSettings : ICommand
    {
        public Guid StudentId { get; }
        public AvailableSettingsDto AvailableSettings { get; }

        public UpdateUserSettings(Guid studentId, AvailableSettingsDto availableSettings)
        {
            StudentId = studentId;
            AvailableSettings = availableSettings ?? throw new ArgumentNullException(nameof(availableSettings));
        }
    }
}

using System;

namespace MiniSpace.Services.Students.Application.Exceptions
{
    public class UserSettingsNotFoundException : AppException
    {
        public override string Code { get; } = "user_settings_not_found";
        public Guid Id { get; }

        public UserSettingsNotFoundException(Guid id) : base($"User settings for student with id: {id} were not found.")
        {
            Id = id;
        }
    }
}

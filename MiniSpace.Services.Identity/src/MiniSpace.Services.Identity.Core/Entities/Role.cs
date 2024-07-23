using System;

namespace MiniSpace.Services.Identity.Core.Entities
{
    public enum Role
    {
        User,
        Admin,
        Banned
    }

    public static class RoleExtensions
    {
        public static bool IsValid(this Role role)
        {
            return Enum.IsDefined(typeof(Role), role);
        }
    }
}
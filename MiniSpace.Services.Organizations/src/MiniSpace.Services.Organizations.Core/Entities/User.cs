namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class User
    {
        public Guid Id { get; }
        public Role Role { get; private set; }

        public User(Guid id, Role role)
        {
            Id = id;
            Role = role ?? throw new ArgumentNullException(nameof(role));
        }

        public bool HasPermission(Permission permission)
        {
            return Role?.Permissions != null && Role.Permissions.ContainsKey(permission) && Role.Permissions[permission];
        }

        public void AssignRole(Role role)
        {
            Role = role ?? throw new ArgumentNullException(nameof(role));
        }
    }
}

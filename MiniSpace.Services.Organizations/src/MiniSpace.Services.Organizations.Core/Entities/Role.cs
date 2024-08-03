namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class Role
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Dictionary<Permission, bool> Permissions { get; private set; }

        public Role(Guid id, string name, string description, Dictionary<Permission, bool> permissions)
        {
            Id = id;
            Name = name;
            Description = description;
            Permissions = permissions ?? new Dictionary<Permission, bool>();
        }

        public Role(string name, string description, Dictionary<Permission, bool> permissions)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Permissions = permissions ?? new Dictionary<Permission, bool>();
        }

        public Role(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Permissions = new Dictionary<Permission, bool>();
        }

        public void UpdatePermissions(Dictionary<Permission, bool> permissions)
        {
            Permissions = permissions;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }
    }
}

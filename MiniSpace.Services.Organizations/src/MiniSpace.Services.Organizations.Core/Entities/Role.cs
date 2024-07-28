namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class Role
    {
        public Guid Id { get; }
        public Guid MemberId { get; }
        public string Name { get; }
        public Dictionary<Permission, bool> Permissions { get; private set; }

        public Role(Guid memberId, string name)
        {
            Id = Guid.NewGuid();
            MemberId = memberId;
            Name = name;
            Permissions = new Dictionary<Permission, bool>();
        }

        public Role(string name, Dictionary<Permission, bool> permissions)
        {
            Id = Guid.NewGuid();
            Name = name;
            Permissions = permissions;
        }

        public void UpdatePermissions(Dictionary<Permission, bool> permissions)
        {
            Permissions = permissions;
        }
    }
}

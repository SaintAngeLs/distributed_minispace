namespace MiniSpace.Services.Organizations.Application.DTO
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public int MemberCount { get; set; }
    }
}


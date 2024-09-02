namespace MiniSpace.Services.Students.Application.Dto
{
    public class BlockedUserDto
    {
        public Guid BlockerId { get; set; }
        public Guid BlockedUserId { get; set; }
        public DateTime BlockedAt { get; set; }
    }
}

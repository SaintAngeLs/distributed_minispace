namespace MiniSpace.Services.Students.Application.Dto
{
    public class BlockedStudentDto
    {
        public Guid BlockerId { get; set; }
        public Guid BlockedUserId { get; set; }
        public string BlockedUserName { get; set; }
        public DateTime BlockedAt { get; set; }
    }
}

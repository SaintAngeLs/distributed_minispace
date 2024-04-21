namespace MiniSpace.Services.Friends.Core.Exceptions
{
    public class StudentFriendCreationException : DomainException
    {
        public override string Code { get; } = "student_friend_creation_failure";
        public Guid StudentId { get; }

        public StudentFriendCreationException(Guid studentId)
            : base($"Failed to create student friend with ID {studentId}.")
        {
            StudentId = studentId;
        }
    }
}

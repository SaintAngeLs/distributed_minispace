namespace MiniSpace.Services.Friends.Core.Exceptions
{
    public class StudentFriendDeletionException : DomainException
    {
        public override string Code { get; } = "student_friend_deletion_failure";
        public Guid StudentId { get; }

        public StudentFriendDeletionException(Guid studentId)
            : base($"Failed to delete student friend with ID {studentId}.")
        {
            StudentId = studentId;
        }
    }
}

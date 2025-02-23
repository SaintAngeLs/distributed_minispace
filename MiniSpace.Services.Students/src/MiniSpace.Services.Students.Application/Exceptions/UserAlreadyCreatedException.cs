namespace MiniSpace.Services.Students.Application.Exceptions
{
    public class UserAlreadyCreatedException : AppException
    {
        public override string Code { get; } = "student_already_created";
        public Guid StudentId { get; }

        public UserAlreadyCreatedException(Guid studentId)
            : base($"Student with id: {studentId} was already created.")
        {
            StudentId = studentId;
        }
    }    
}

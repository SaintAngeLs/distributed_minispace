namespace MiniSpace.Services.MediaFiles.Application.Exceptions
{
    public class StudentAlreadyCreatedException : AppException
    {
        public override string Code { get; } = "student_already_created";
        public Guid StudentId { get; }

        public StudentAlreadyCreatedException(Guid studentId)
            : base($"Student with id: {studentId} was already created.")
        {
            StudentId = studentId;
        }
    }    
}

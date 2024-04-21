namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class InvalidStudentFullNameException : DomainException
    {
        public override string Code { get; } = "invalid_student_full_name";
        public Guid Id { get; }
        public string FullName { get; }
        
        public InvalidStudentFullNameException(Guid id, string fullName) : base(
            $"Student with id: {id} has invalid full name.")
        {
            Id = id;
            FullName = fullName;
        }
    }
}

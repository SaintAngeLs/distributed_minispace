namespace MiniSpace.Services.MediaFiles.Core.Exceptions
{
    public class InvalidStudentDescriptionException : DomainException
    {
        public override string Code { get; } = "invalid_student_description";
        public Guid Id { get; }
        public string Description { get; }
        
        public InvalidStudentDescriptionException(Guid id, string description) : base(
            $"Student with id: {id} has invalid description.")
        {
            Id = id;
            Description = description;
        }
    }
}

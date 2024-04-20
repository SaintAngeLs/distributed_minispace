namespace MiniSpace.Services.Posts.Application.Exceptions
{
    public class StudentNotFoundException : AppException
    {
        public override string Code { get; } = "student_not_found";
        public Guid Id { get; }

        public StudentNotFoundException(Guid id) : base($"Student with id: {id} was not found.")
        {
            Id = id;
        }
    }    
}

namespace MiniSpace.Services.Students.Application.Exceptions
{
    public class UserNotFoundException : AppException
    {
        public override string Code { get; } = "student_not_found";
        public Guid Id { get; }

        public UserNotFoundException(Guid id) : base($"User with id: {id} was not found.")
        {
            Id = id;
        }
    }    
}

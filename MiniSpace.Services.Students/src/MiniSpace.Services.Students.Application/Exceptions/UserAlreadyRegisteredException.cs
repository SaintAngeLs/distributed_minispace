namespace MiniSpace.Services.Students.Application.Exceptions
{
    public class UserAlreadyRegisteredException : AppException
    {
        public override string Code { get; } = "student_already_registered";
        public Guid Id { get; }
        
        public UserAlreadyRegisteredException(Guid id) 
            : base($"Student with id: {id} has already been registered.")
        {
            Id = id;
        }
    }    
}

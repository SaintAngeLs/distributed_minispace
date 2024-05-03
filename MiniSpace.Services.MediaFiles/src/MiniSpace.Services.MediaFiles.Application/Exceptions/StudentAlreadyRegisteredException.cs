namespace MiniSpace.Services.MediaFiles.Application.Exceptions
{
    public class StudentAlreadyRegisteredException : AppException
    {
        public override string Code { get; } = "student_already_registered";
        public Guid Id { get; }
        
        public StudentAlreadyRegisteredException(Guid id) 
            : base($"Student with id: {id} has already been registered.")
        {
            Id = id;
        }
    }    
}

namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class InvalidStudentServiceClientResponseException : DomainException
    {
        public override string Code { get; } = "invalid_student_service_client_response";
        public Guid StudentId { get; }

        public InvalidStudentServiceClientResponseException(Guid studentId) 
            : base($"Invalid student service client response for student with ID: '{studentId}'.")
        {
            StudentId = studentId;
        }
    }
}
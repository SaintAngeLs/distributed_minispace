namespace MiniSpace.Services.Reactions.Application.Exceptions
{
    public class UnauthorizedIdentityException : AppException
    {
        public override string Code { get; } = "student_not_found";
        public Guid Id { get; }

        public UnauthorizedIdentityException(Guid id) : base($"Identity with id: {id} is not authorized.")
        {
            Id = id;
        }
    }    
}

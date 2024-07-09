namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class InvalidEducationException : DomainException
    {
        public override string Code { get; } = "invalid_education";
        public Guid Id { get; }

        public InvalidEducationException(Guid id) : base($"Student with id: {id} has invalid education information.")
        {
            Id = id;
        }
    }
}
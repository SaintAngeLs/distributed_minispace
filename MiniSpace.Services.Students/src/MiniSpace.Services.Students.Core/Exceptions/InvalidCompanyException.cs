namespace MiniSpace.Services.Students.Core.Exceptions
{
     public class InvalidCompanyException : DomainException
    {
        public override string Code { get; } = "invalid_company";
        public Guid Id { get; }

        public InvalidCompanyException(Guid id) : base($"Student with id: {id} has an invalid company.")
        {
            Id = id;
        }
    }
}
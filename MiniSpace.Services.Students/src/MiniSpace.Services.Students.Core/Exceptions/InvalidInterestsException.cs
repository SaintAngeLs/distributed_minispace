namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class InvalidInterestsException : DomainException
    {
        public override string Code { get; } = "invalid_interests";
        public Guid Id { get; }

        public InvalidInterestsException(Guid id) : base($"Student with id: {id} has invalid interests.")
        {
            Id = id;
        }
    }
}
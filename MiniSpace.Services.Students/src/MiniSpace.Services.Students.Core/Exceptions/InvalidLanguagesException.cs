namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class InvalidLanguagesException : DomainException
    {
        public override string Code { get; } = "invalid_languages";
        public Guid Id { get; }

        public InvalidLanguagesException(Guid id) : base($"Student with id: {id} has invalid languages.")
        {
            Id = id;
        }
    }
}
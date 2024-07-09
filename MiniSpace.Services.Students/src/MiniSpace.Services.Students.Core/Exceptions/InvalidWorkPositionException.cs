namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class InvalidWorkPositionException : DomainException
    {
        public override string Code { get; } = "invalid_work_position";
        public Guid Id { get; }

        public InvalidWorkPositionException(Guid id) : base($"Student with id: {id} has an invalid work position.")
        {
            Id = id;
        }
    }
}
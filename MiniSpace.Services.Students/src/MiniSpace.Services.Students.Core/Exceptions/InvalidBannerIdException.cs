namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class InvalidBannerIdException : DomainException
    {
        public override string Code { get; } = "invalid_banner_id";
        public Guid Id { get; }

        public InvalidBannerIdException(Guid id) : base($"Student with id: {id} has an invalid banner ID.")
        {
            Id = id;
        }
    }
}
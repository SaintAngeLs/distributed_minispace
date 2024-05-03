using MiniSpace.Services.MediaFiles.Core.Entities;

namespace MiniSpace.Services.MediaFiles.Core.Exceptions
{
    public class CannotUpdateStudentException : DomainException
    {
        public override string Code { get; } = "cannot_update_student";
        public Guid Id { get; }

        public CannotUpdateStudentException(Guid id) : base(
            $"Cannot update student: {id}.")
        {
            Id = id;
        }
    }
}
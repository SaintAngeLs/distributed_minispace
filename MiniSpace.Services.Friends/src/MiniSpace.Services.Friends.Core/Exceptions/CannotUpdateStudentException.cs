using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Exceptions
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
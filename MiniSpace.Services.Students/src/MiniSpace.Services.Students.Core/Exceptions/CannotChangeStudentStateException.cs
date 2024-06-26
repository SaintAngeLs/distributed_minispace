using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class CannotChangeStudentStateException : DomainException
    {
        public override string Code { get; } = "cannot_change_student_state";
        public Guid Id { get; }
        public State State { get; }

        public CannotChangeStudentStateException(Guid id, State state) : base(
            $"Cannot change student: {id} state to: {state}.")
        {
            Id = id;
            State = state;
        }
    }
}

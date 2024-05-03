using MiniSpace.Services.MediaFiles.Core.Entities;

namespace MiniSpace.Services.MediaFiles.Core.Exceptions
{
    public class StudentStateAlreadySetException : DomainException
    {
        public override string Code { get; } = "student_state_already_set";
        public Guid Id { get; }
        public State State { get; }

        public StudentStateAlreadySetException(Guid id, State state) : base(
            $"Student: {id} has state already set to: {state}.")
        {
            Id = id;
            State = state;
        }
    }
}

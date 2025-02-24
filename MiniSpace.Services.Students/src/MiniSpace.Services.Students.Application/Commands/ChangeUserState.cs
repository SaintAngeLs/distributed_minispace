using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class ChangeUserState : ICommand
    {
        public Guid UserId { get; }
        public string State { get; }

        public ChangeUserState(Guid userId, string state)
        {
            UserId = userId;
            State = state;
        }
    }    
}

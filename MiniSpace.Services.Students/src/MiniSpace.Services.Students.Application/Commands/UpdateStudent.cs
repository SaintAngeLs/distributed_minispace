using Convey.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UpdateStudent : ICommand
    {
        public Guid Id { get; }
        public bool IsBanned { get; private set; }
        public bool CanCreateEvents { get; private set; }

        public UpdateStudent(Guid id, bool isBanned, bool canCreateEvents)
        {
            Id = id;
            IsBanned = isBanned;
            CanCreateEvents = canCreateEvents;
        }
    }    
}

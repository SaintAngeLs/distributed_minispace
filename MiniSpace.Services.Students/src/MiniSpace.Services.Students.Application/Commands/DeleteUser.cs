using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class DeleteUser : ICommand
    {
        public Guid UserId { get; }

        public DeleteUser(Guid userId) => UserId = userId;
    }    
}

using Convey.CQRS.Commands;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Application.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class BlockUserHandler : ICommandHandler<BlockUser>
    {
        private readonly IBlockedUsersRepository _blockedUsersRepository;
        private readonly IStudentRepository _studentRepository;

        public BlockUserHandler(IBlockedUsersRepository blockedUsersRepository, IStudentRepository studentRepository)
        {
            _blockedUsersRepository = blockedUsersRepository;
            _studentRepository = studentRepository;
        }

        public async Task HandleAsync(BlockUser command, CancellationToken cancellationToken = default)
        {
            var blocker = await _studentRepository.GetAsync(command.BlockerId);
            var blockedUser = await _studentRepository.GetAsync(command.BlockedUserId);

            if (blocker is null)
            {
                throw new StudentNotFoundException(command.BlockerId);
            }

            if (blockedUser is null)
            {
                throw new StudentNotFoundException(command.BlockedUserId);
            }

            var existingBlockedUser = await _blockedUsersRepository.GetAsync(command.BlockerId, command.BlockedUserId);
            if (existingBlockedUser != null)
            {
                throw new UserAlreadyBlockedException(command.BlockerId, command.BlockedUserId);
            }

            var blockedUserEntity = new BlockedUser(command.BlockerId, command.BlockedUserId, DateTime.UtcNow);
            await _blockedUsersRepository.AddAsync(blockedUserEntity);
        }
    }
}

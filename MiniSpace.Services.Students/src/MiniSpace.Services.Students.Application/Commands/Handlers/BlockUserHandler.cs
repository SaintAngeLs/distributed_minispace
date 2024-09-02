using Convey.CQRS.Commands;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class BlockUserHandler : ICommandHandler<BlockUser>
    {
        private readonly IBlockedUsersRepository _blockedUsersRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public BlockUserHandler(IBlockedUsersRepository blockedUsersRepository, IStudentRepository studentRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _blockedUsersRepository = blockedUsersRepository;
            _studentRepository = studentRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
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

            var blockedUsersAggregate = await _blockedUsersRepository.GetAsync(command.BlockerId);
            if (blockedUsersAggregate == null)
            {
                blockedUsersAggregate = new BlockedUsers(command.BlockerId);
            }

            blockedUsersAggregate.BlockUser(command.BlockedUserId);

            if (!blockedUsersAggregate.BlockedUsersList.Any())
            {
                await _blockedUsersRepository.AddAsync(blockedUsersAggregate);
            }
            else
            {
                await _blockedUsersRepository.UpdateAsync(blockedUsersAggregate);
            }

            var events = _eventMapper.MapAll(blockedUsersAggregate.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}

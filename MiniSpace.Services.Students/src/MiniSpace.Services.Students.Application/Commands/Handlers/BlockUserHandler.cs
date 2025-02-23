using Paralax.CQRS.Commands;
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
        private readonly IUserRepository _userRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public BlockUserHandler(IBlockedUsersRepository blockedUsersRepository, IUserRepository userRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _blockedUsersRepository = blockedUsersRepository;
            _userRepository = userRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(BlockUser command, CancellationToken cancellationToken = default)
        {
            // Ensure the blocker exists
            var blocker = await _userRepository.GetAsync(command.BlockerId);
            if (blocker is null)
            {
                throw new UserNotFoundException(command.BlockerId);
            }

            // Ensure the user to be blocked exists
            var blockedUser = await _userRepository.GetAsync(command.BlockedUserId);
            if (blockedUser is null)
            {
                throw new UserNotFoundException(command.BlockedUserId);
            }

            // Fetch or create the BlockedUsers aggregate
            var blockedUsersAggregate = await _blockedUsersRepository.GetAsync(command.BlockerId);
            if (blockedUsersAggregate == null)
            {
                blockedUsersAggregate = new BlockedUsers(command.BlockerId);
                await _blockedUsersRepository.AddAsync(blockedUsersAggregate);
            }

            // Block the user
            blockedUsersAggregate.BlockUser(command.BlockedUserId);

            // Update the repository to save changes
            await _blockedUsersRepository.UpdateAsync(blockedUsersAggregate);

            // Publish domain events
            var events = _eventMapper.MapAll(blockedUsersAggregate.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}

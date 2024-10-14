using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Identity.Application.DTO;
using MiniSpace.Services.Identity.Application.Events;
using MiniSpace.Services.Identity.Application.Exceptions;
using MiniSpace.Services.Identity.Core.Entities;
using MiniSpace.Services.Identity.Core.Exceptions;
using MiniSpace.Services.Identity.Core.Repositories;

namespace MiniSpace.Services.Identity.Application.Services.Identity
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRng _rng;
        private readonly IMessageBroker _messageBroker;
        private readonly IIPAddressService _ipAddressService; 

        public RefreshTokenService(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IJwtProvider jwtProvider,
            IRng rng,
            IMessageBroker messageBroker,
            IIPAddressService ipAddressService) 
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _rng = rng;
            _messageBroker = messageBroker;
            _ipAddressService = ipAddressService; 
        }

        public async Task<string> CreateAsync(Guid userId)
        {
            var token = _rng.Generate(30, true);
            var refreshToken = new RefreshToken(new AggregateId(), userId, token, DateTime.UtcNow);
            await _refreshTokenRepository.AddAsync(refreshToken);

            return token;
        }

        public async Task RevokeAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetAsync(refreshToken);
            if (token is null)
            {
                throw new InvalidRefreshTokenException();
            }

            token.Revoke(DateTime.UtcNow);
            await _refreshTokenRepository.UpdateAsync(token);

            var user = await _userRepository.GetAsync(token.UserId);
            if (user != null)
            {
                user.SetOnlineStatus(false, null); 
                await _userRepository.UpdateAsync(user);

                await _messageBroker.PublishAsync(new SignedOut(user.Id, user.DeviceType));
            }
        }

        public async Task<AuthDto> UseAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetAsync(refreshToken);
            if (token is null || token.Revoked)
            {
                throw new InvalidRefreshTokenException();
            }

            var user = await _userRepository.GetAsync(token.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(token.UserId);
            }

            var claims = user.Permissions.Any()
                ? new Dictionary<string, IEnumerable<string>> { ["permissions"] = user.Permissions }
                : null;

            var auth = _jwtProvider.Create(token.UserId, user.Role, claims: claims);
            auth.RefreshToken = refreshToken;

            var ipAddress = _ipAddressService.GetIPAddress();
            await _messageBroker.PublishAsync(new TokenRefreshed(user.Id, user.DeviceType, ipAddress));

            return auth;
        }
    }
}

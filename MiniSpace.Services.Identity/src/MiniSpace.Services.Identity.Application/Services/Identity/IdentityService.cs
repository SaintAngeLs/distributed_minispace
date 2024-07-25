using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Identity.Application.Commands;
using MiniSpace.Services.Identity.Application.DTO;
using MiniSpace.Services.Identity.Application.Events;
using MiniSpace.Services.Identity.Application.Events.Rejected;
using MiniSpace.Services.Identity.Application.Exceptions;
using MiniSpace.Services.Identity.Core.Entities;
using MiniSpace.Services.Identity.Core.Exceptions;
using MiniSpace.Services.Identity.Core.Repositories;

namespace MiniSpace.Services.Identity.Application.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private readonly IUserRepository _userRepository;
        private readonly IUserResetTokenRepository _userResetTokenRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMessageBroker _messageBroker;
        private readonly IVerificationTokenService _verificationTokenService;
        private readonly ITwoFactorSecretTokenService _twoFactorSecretTokenService;
        private readonly ITwoFactorCodeService _twoFactorCodeService;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(IUserRepository userRepository, IPasswordService passwordService,
            IJwtProvider jwtProvider, IRefreshTokenService refreshTokenService,
            IMessageBroker messageBroker, IUserResetTokenRepository userResetTokenRepository,
            IVerificationTokenService verificationTokenService, 
            ITwoFactorSecretTokenService twoFactorSecretTokenService,
            ITwoFactorCodeService  twoFactorCodeService,
            ILogger<IdentityService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
            _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
            _messageBroker = messageBroker ?? throw new ArgumentNullException(nameof(messageBroker));
            _userResetTokenRepository = userResetTokenRepository ?? throw new ArgumentNullException(nameof(userResetTokenRepository));
            _verificationTokenService = verificationTokenService ?? throw new ArgumentNullException(nameof(verificationTokenService));
            _twoFactorSecretTokenService = twoFactorSecretTokenService ?? throw new ArgumentNullException(nameof(twoFactorSecretTokenService));
            _twoFactorCodeService = twoFactorCodeService ?? throw new ArgumentNullException(nameof(twoFactorCodeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            return user is null ? null : new UserDto(user);
        }

        public async Task<AuthDto> SignInAsync(SignIn command)
        {
            if (!EmailRegex.IsMatch(command.Email))
            {
                throw new InvalidEmailException(command.Email);
            }

            var user = await _userRepository.GetAsync(command.Email);
            if (user is null || !_passwordService.IsValid(user.Password, command.Password))
            {
                throw new InvalidCredentialsException(command.Email);
            }

            if (user.IsTwoFactorEnabled)
            {
                var code = _twoFactorCodeService.GenerateCode(user.TwoFactorSecret);
                await _messageBroker.PublishAsync(new TwoFactorCodeGenerated(user.Id, code));

                return new AuthDto
                {
                    IsTwoFactorRequired = true,
                    UserId = user.Id
                };
            }

            var claims = new Dictionary<string, IEnumerable<string>>
            {
                ["name"] = new[] { user.Name },
                ["e-mail"] = new[] { user.Email }
            };
            if (user.Permissions.Any())
            {
                claims.Add("permissions", user.Permissions);
            }
            var auth = _jwtProvider.Create(user.Id, user.Role, claims: claims);
            auth.RefreshToken = await _refreshTokenService.CreateAsync(user.Id);

            await _messageBroker.PublishAsync(new SignedIn(user.Id, user.Role));

            return auth;
        }



        public async Task SignUpAsync(SignUp command)
        {
            if (!EmailRegex.IsMatch(command.Email))
            {
                _logger.LogError($"Invalid email: {command.Email}");
                throw new InvalidEmailException(command.Email);
            }

            var user = await _userRepository.GetAsync(command.Email);
            if (user is {})
            {
                _logger.LogError($"Email already in use: {command.Email}");
                throw new EmailInUseException(command.Email);
            }

            var role = string.IsNullOrWhiteSpace(command.Role) ? Role.User : Enum.Parse<Role>(command.Role, true);
            var password = _passwordService.Hash(command.Password);
            user = new User(command.UserId, $"{command.FirstName} {command.LastName}", command.Email, password,
                role, DateTime.UtcNow, command.Permissions);

            var (token, hashedToken) = _verificationTokenService.GenerateToken(user.Id, user.Email);
            user.SetEmailVerificationToken(hashedToken);

            await _userRepository.AddAsync(user);

            _logger.LogInformation($"Created an account for the user with id: {user.Id}.");

            await _messageBroker.PublishAsync(new SignedUp(user.Id, command.FirstName, command.LastName, 
                user.Email, user.Role.ToString(), token, hashedToken));
        }

        public async Task BanUserAsync(BanUser command)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                _logger.LogError($"User with id: {command.UserId} was not found.");
                throw new UserNotFoundException(command.UserId);
            }

            user.Ban();
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"Banned the user with id: {user.Id}.");
            await _messageBroker.PublishAsync(new UserBanned(user.Id));
        }

        public async Task UnbanUserAsync(UnbanUser command)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                _logger.LogError($"User with id: {command.UserId} was not found.");
                throw new UserNotFoundException(command.UserId);
            }

            user.Unban();
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"Unbanned the user with id: {user.Id}.");
            await _messageBroker.PublishAsync(new UserUnbanned(user.Id));
        }

        public async Task ForgotPasswordAsync(ForgotPassword command)
        {
            var user = await _userRepository.GetAsync(command.Email);
            if (user == null)
            {
                _logger.LogError($"No user associated with email: {command.Email}");
                throw new UserNotFoundException(command.UserId);
            }

            var resetToken = _jwtProvider.GenerateResetToken(user.Id);
            var userResetToken = new UserResetToken(user.Id, resetToken, DateTime.UtcNow.AddDays(1));
            await _messageBroker.PublishAsync(new PasswordResetTokenGenerated(user.Id, command.Email, resetToken));

            _logger.LogInformation($"Reset token generated for user id: {user.Id}");

            await _userResetTokenRepository.SaveAsync(userResetToken);
        }

        public async Task ResetPasswordAsync(ResetPassword command)
        {
            if (command.UserId == Guid.Empty)
            {
                _logger.LogError("Reset password attempt failed: User ID is empty.");
                throw new UserNotFoundException(command.UserId);
            }

            _logger.LogInformation("Fetching user reset token from repository...");
            var userResetToken = await _userResetTokenRepository.GetByUserIdAsync(command.UserId);

            if (userResetToken == null || !userResetToken.ResetTokenIsValid(command.Token))
            {
                _logger.LogError($"Invalid or expired reset token for user ID: {command.UserId}");
                throw new InvalidTokenException();
            }

            var user = await _userRepository.GetAsync(userResetToken.UserId);
            if (user == null)
            {
                _logger.LogError($"User not found for ID: {command.UserId}");
                throw new UserNotFoundException(command.UserId);
            }

            _logger.LogInformation("Updating user's password...");
            user.Password = _passwordService.Hash(command.NewPassword);
            await _userRepository.UpdateAsync(user);
            await _userResetTokenRepository.InvalidateTokenAsync(user.Id);

            await _messageBroker.PublishAsync(new PasswordReset(user.Id));
        }

        public async Task VerifyEmailAsync(VerifyEmail command)
        {
            var user = await _userRepository.GetAsync(command.Email);
            if (user == null)
            {
                _logger.LogError($"No user associated with email: {command.Email}");
                throw new UserNotFoundByEmailException(command.Email);
            }

            if (!_verificationTokenService.ValidateToken(command.Token, command.HashedToken))
            {
                _logger.LogError($"Invalid verification token for email: {command.Email}");
                throw new InvalidTokenException();
            }

            user.VerifyEmail();
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"Email verified for user id: {user.Id}");
            await _messageBroker.PublishAsync(new EmailVerified(user.Id, user.Email, DateTime.UtcNow));
        }


        public async Task EnableTwoFactorAsync(EnableTwoFactor command)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user == null)
            {
                _logger.LogError($"User with id: {command.UserId} was not found.");
                throw new UserNotFoundException(command.UserId);
            }

            user.EnableTwoFactorAuthentication(command.Secret);
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"Two-factor authentication enabled for user id: {user.Id}");
            await _messageBroker.PublishAsync(new TwoFactorAuthenticationEnabled(user.Id, command.Secret));
        }

        public async Task DisableTwoFactorAsync(DisableTwoFactor command)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user == null)
            {
                _logger.LogError($"User with id: {command.UserId} was not found.");
                throw new UserNotFoundException(command.UserId);
            }

            user.DisableTwoFactorAuthentication();
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"Two-factor authentication disabled for user id: {user.Id}");
            await _messageBroker.PublishAsync(new TwoFactorAuthenticationDisabled(user.Id));
        }

        public async Task<string> GenerateTwoFactorSecretAsync(GenerateTwoFactorSecret command)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            var secret = _twoFactorSecretTokenService.GenerateSecret();

            user.SetTwoFactorSecret(secret);
            await _userRepository.UpdateAsync(user);

            return secret;
        }

        public async Task<AuthDto> VerifyTwoFactorCodeAsync(VerifyTwoFactorCode command)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            Console.WriteLine($"User retrieved: {JsonSerializer.Serialize(user)}"); 
            if (user == null)
            {
                throw new UserNotFoundException(command.UserId);
            }
            Console.WriteLine($"Code received is {command.Code}");

            bool isValidCode = _twoFactorCodeService.ValidateCode(user.TwoFactorSecret, command.Code);
            Console.WriteLine($"Is the 2FA code valid? {isValidCode}");

            if (!isValidCode)
            {
                throw new InvalidTwoFactorCodeException();
            }

            var claims = new Dictionary<string, IEnumerable<string>>
            {
                ["name"] = new[] { user.Name },
                ["e-mail"] = new[] { user.Email }
            };
            if (user.Permissions.Any())
            {
                claims.Add("permissions", user.Permissions);
            }
            var auth = _jwtProvider.Create(user.Id, user.Role, claims: claims);
            auth.RefreshToken = await _refreshTokenService.CreateAsync(user.Id);

            await _messageBroker.PublishAsync(new SignedIn(user.Id, user.Role));
            return auth;
        }


    }
}

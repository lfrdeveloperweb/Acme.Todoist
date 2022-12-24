using Acme.Todoist.Application.Core.Cryptography;
using Acme.Todoist.Application.Core.Security;
using Acme.Todoist.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Acme.Todoist.Application.Settings;

namespace Acme.Todoist.Infrastructure.Security
{
    internal class SecurityService : ISecurityService
    {
        private readonly ILogger<SecurityService> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly AccountSettings _accountSettings;

        public SecurityService(
            ILogger<SecurityService> logger,
            IPasswordHasher passwordHasher,
            IOptionsSnapshot<AccountSettings> accountSettings)
        {
            _logger = logger;
            _passwordHasher = passwordHasher;
            _accountSettings = accountSettings.Value;
        }

        public async Task<SignInResult> CheckPasswordAsync(User user, string password)
        {
            if (user.IsLocked) return SignInResult.LockedOut;
            if (!user.EmailConfirmed) return SignInResult.EmailNotConfirmed;
            if (!user.PhoneNumberConfirmed) return SignInResult.PhoneNumberNotConfirmed;

            if (_passwordHasher.VerifyHashedPassword(user.PasswordHash, password)) return SignInResult.Success;

            return _accountSettings.MaxFailedAccessAttempts >= user.AccessFailedCount + 1 
                ? SignInResult.LockedOut 
                : SignInResult.LoginFailed;
        }
    }
}

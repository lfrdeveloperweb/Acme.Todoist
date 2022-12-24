using Acme.Todoist.Application.Services;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Security;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Acme.Todoist.Infrastructure.Security
{
    internal class JwtProvider : IJwtProvider
    {
        private readonly JwtSettings _settings;
        private readonly ISystemClock _systemClock;

        public JwtProvider(IOptionsSnapshot<JwtSettings> settings, ISystemClock systemClock)
        {
            _settings = settings.Value;
            _systemClock = systemClock;
        }

        /// <inheritdoc />
        public JwtToken Generate(User user)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var claims = new Dictionary<string, object>
            {
                [JwtClaimTypes.Subject] = user.Id,
                [JwtClaimTypes.Name] = user.Name,
                [JwtClaimTypes.Email] = user.Email,
                [JwtClaimTypes.Role] = user.Role,
            };

            var tokenExpiration = TimeSpan.FromMinutes(_settings.TokenExpirationInMinutes);
            var tokenExpirationTime = _systemClock.UtcNow.Add(tokenExpiration).DateTime;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Subject = new ClaimsIdentity(claims),
                Expires = tokenExpirationTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecurityKey)), SecurityAlgorithms.HmacSha256),
                Audience = _settings.Audience,
                Issuer = _settings.Issuer,
                Claims = claims
            };

            var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            string tokenValue = jwtSecurityTokenHandler.WriteToken(token);

            return new JwtToken(tokenValue, JwtBearerDefaults.AuthenticationScheme, (int) tokenExpiration.TotalSeconds);
        }
    }
}

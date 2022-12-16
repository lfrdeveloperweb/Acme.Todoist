using Acme.Todoist.Application.Core.Commons;
using Acme.Todoist.Application.Services;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Acme.Todoist.Infrastructure.Security
{
    internal class JwtProvider : IJwtProvider
    {
        private readonly JwtSettings _settings;
        private readonly IDateTimeProvider _dateTimeProvider;

        public JwtProvider(IOptionsSnapshot<JwtSettings> settings, IDateTimeProvider dateTimeProvider)
        {
            _settings = settings.Value;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <inheritdoc />
        public JwtToken Generate(User user)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Name, user.Name)
            };

            var tokenExpiration = TimeSpan.FromMinutes(_settings.TokenExpirationInMinutes);
            var tokenExpirationTime = _dateTimeProvider.UtcNow.Add(tokenExpiration).DateTime;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = tokenExpirationTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecurityKey)),
                    SecurityAlgorithms.HmacSha256),
                Audience = _settings.Audience,
                Issuer = _settings.Issuer,

            };

            var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

            string tokenValue = jwtSecurityTokenHandler.WriteToken(token);

            return new JwtToken(tokenValue, JwtBearerDefaults.AuthenticationScheme, (int) tokenExpiration.TotalSeconds);
        }
    }
}

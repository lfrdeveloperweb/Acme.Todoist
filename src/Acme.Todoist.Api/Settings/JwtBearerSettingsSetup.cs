using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Acme.Todoist.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Acme.Todoist.Api.Settings
{
    public sealed class JwtBearerSettingsSetup : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtSettings _jwtSettings;

        public JwtBearerSettingsSetup(IOptionsSnapshot<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public void Configure(JwtBearerOptions options)
        {
            //options.RequireHttpsMetadata = false;
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey)),
                NameClaimType = JwtRegisteredClaimNames.Name,
                RoleClaimType = "role"
            };

            options.Events = new JwtBearerEvents
            {
                //OnAuthenticationFailed = async context =>
                //{

                //},
                OnTokenValidated = async context =>
                {
                    var accessToken = context.SecurityToken as JwtSecurityToken;
                    if (Guid.TryParse(accessToken.Id, out Guid sessionId))
                    {
                        //if (await _session.ValidateSessionAsync(sessionId))
                        //{
                        //    return;
                        //}
                    }

                    throw new SecurityTokenValidationException("Session not valid for provided token.");
                }
            };
        }

        public void Configure(string name, JwtBearerOptions options) => Configure(options);
    }
}

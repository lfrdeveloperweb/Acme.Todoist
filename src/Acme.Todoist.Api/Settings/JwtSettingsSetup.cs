using Acme.Todoist.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Acme.Todoist.Api.Settings
{
    internal sealed class JwtSettingsSetup : IConfigureOptions<JwtSettings>
    {
        private const string SectionName = "security:jwtSettings";

        private readonly IConfiguration _configuration;

        public JwtSettingsSetup(IConfiguration configuration) => _configuration = configuration;

        public void Configure(JwtSettings options) => _configuration.GetSection(SectionName).Bind(options);
    }
}

using Acme.Todoist.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Acme.Todoist.Api.Settings;

public sealed class SettingsSetup : 
    IConfigureOptions<AccountSettings>,
    IConfigureOptions<JwtSettings>
{
    private const string AccountSettingsSectionName = "security:accountSettings";
    private const string JwtSettingsSectionName = "security:jwtSettings";

    private readonly IConfiguration _configuration;

    public SettingsSetup(IConfiguration configuration) => _configuration = configuration;

    public void Configure(AccountSettings options) => _configuration.GetSection(AccountSettingsSectionName).Bind(options);

    public void Configure(JwtSettings options) => _configuration.GetSection(JwtSettingsSectionName).Bind(options);
}
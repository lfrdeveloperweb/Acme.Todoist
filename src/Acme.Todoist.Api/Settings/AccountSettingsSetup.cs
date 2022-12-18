using Acme.Todoist.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Acme.Todoist.Api.Settings;

internal sealed class AccountSettingsSetup : IConfigureOptions<AccountSettings>
{
    private const string SectionName = "security:accountSettings";

    private readonly IConfiguration _configuration;

    public AccountSettingsSetup(IConfiguration configuration) => _configuration = configuration;

    public void Configure(AccountSettings options) => _configuration.GetSection(SectionName).Bind(options);
}
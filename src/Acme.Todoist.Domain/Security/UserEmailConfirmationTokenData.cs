namespace Acme.Todoist.Domain.Security;

public sealed record UserEmailConfirmationTokenData(string Email) : IUserTokenData
{
    public UserTokenType TokenType => UserTokenType.EmailConfirmationToken;
}
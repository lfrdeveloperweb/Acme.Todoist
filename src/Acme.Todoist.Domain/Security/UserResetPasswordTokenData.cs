namespace Acme.Todoist.Domain.Security;

public sealed record UserResetPasswordTokenData(string SocialSecurityNumber) : IUserTokenData
{
    public UserTokenType TokenType => UserTokenType.ResetPasswordToken;
}
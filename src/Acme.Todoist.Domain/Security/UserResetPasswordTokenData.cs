namespace Acme.Todoist.Domain.Security;

public sealed record UserResetPasswordTokenData(string DocumentNumber) : IUserTokenData
{
    public UserTokenType TokenType => UserTokenType.ResetPasswordToken;
}
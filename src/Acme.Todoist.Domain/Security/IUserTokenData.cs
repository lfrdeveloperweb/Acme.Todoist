namespace Acme.Todoist.Domain.Security;

public interface IUserTokenData
{
    /// <summary>
    /// Token type.
    /// </summary>
    UserTokenType TokenType { get; }
}
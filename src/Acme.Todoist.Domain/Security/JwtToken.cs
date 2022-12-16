namespace Acme.Todoist.Domain.Security
{
    public sealed record JwtToken(string Token, string TokenType, int ExpiresIn);
}

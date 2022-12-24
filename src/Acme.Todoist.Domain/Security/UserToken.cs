using System;

namespace Acme.Todoist.Domain.Security;

/// <summary>
/// Represent a token for a user.
/// </summary>
public class UserToken
{
    protected UserToken() { }

    public UserToken(string userId, UserTokenType type, string value, DateTimeOffset expiresAt)
    {
        UserId = userId;
        Type = type;
        Value = value;
        ExpiresAt = expiresAt;
    }

    public string UserId { get; init; }

    public UserTokenType Type { get; init; }

    public string Value { get; init; }

    public DateTimeOffset ExpiresAt { get; init; }
}

public sealed class UserToken<TUserTokenData> : UserToken where TUserTokenData : IUserTokenData
{
    protected UserToken() { }

    public UserToken(string userId, string value, DateTimeOffset expiresAt, UserTokenType type, string data)
        : base(userId, type, value, expiresAt)
    {
        Data = data;
    }

    /// <summary>
    /// Data of token.
    /// </summary>
    public string Data { get; set; }
}
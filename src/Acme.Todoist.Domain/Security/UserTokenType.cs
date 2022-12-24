namespace Acme.Todoist.Domain.Security;

/// <summary>
/// Represent token types.
/// </summary>
public enum UserTokenType : byte
{
    /// <summary>
    /// Token for confirmation phone number.
    /// </summary>
    PhoneNumberConfirmationToken = 1,

    /// <summary>
    /// Token for confirmation email.
    /// </summary>
    EmailConfirmationToken = 2,

    /// <summary>
    /// Token for reset password.
    /// </summary>
    ResetPasswordToken = 3
}
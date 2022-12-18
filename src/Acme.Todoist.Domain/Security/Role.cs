namespace Acme.Todoist.Domain.Security;

/// <summary>
/// Represent roles that user can to have.
/// </summary>
public enum Role : byte
{
    Anonymous = 0,
    Admin = 1,
    User = 2
}
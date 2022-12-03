namespace Acme.Todoist.Commons.Models.Security;

/// <summary>
/// Represent roles that user can to have.
/// </summary>
public enum Role : byte
{
    Anonymous = 0,
    Admin = 1,
    Customer = 10,
    ClientApplication = 99
}
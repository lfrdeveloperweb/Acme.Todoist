using System.ComponentModel;

namespace Acme.Todoist.Domain.Models
{
    /// <summary>
    /// Represent roles that user can to have.
    /// </summary>
    public enum Role : byte
    {
        [Description("Administrador")]
        Admin = 1,

        [Description("Review")]
        Customer = 2
    }
}

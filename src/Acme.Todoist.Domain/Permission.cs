using System.ComponentModel;

namespace Acme.Todoist.Domain
{
    /// <summary>
    /// Represent scope that a user can access.
    /// </summary>
    public enum Permission : byte
    {
        [Description("user.read")]
        UserRead = 1,

        [Description("user.create")]
        UserCreate = 2,

        [Description("user.update")]
        UserUpdate = 3,

        [Description("user.delete")]
        UserDelete = 4,

        [Description("user.lock")]
        UserLock = 5,

        [Description("user.unlock")]
        UserUnlock = 6,


        [Description("todo.read")]
        TodoRead = 100,

        [Description("todo.create")]
        TodoCreate = 101,

        [Description("todo.update")]
        TodoUpdate = 102,

        [Description("todo.delete")]
        TodoDelete = 103,
    }
}

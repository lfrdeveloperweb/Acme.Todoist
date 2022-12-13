namespace Acme.Todoist.Domain.Models
{
    public sealed class User : EntityBase
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; }

        public string Password { get; }
    }
}

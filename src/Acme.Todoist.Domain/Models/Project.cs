namespace Acme.Todoist.Domain.Models
{
    public sealed class Project : EntityBase
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
    }
}

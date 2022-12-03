namespace Acme.Todoist.Domain.Models
{
    public sealed class Todo : EntityBase
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public Project Project { get; set; }
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
    }
}

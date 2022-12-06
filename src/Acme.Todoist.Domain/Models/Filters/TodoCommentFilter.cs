namespace Acme.Todoist.Domain.Models.Filters
{
    public sealed record TodoCommentFilter
    {
        public string TodoId { get; init; }
    }
}
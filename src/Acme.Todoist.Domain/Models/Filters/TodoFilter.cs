namespace Acme.Todoist.Domain.Models.Filters
{
    public sealed record TodoFilter
    {
        public bool? IsCompleted { get; init; }
        
        public bool IsDeleted { get; init; }
    }
}

using System;
using System.Collections.Generic;

namespace Acme.Todoist.Domain.Models
{
    public sealed class Todo : EntityBase, ICloneable
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; }
        //public int ViewsCount { get; set; }
        public ICollection<string> Labels { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        
        public object Clone() => this.MemberwiseClone();
    }
}

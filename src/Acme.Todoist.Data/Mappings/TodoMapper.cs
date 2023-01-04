using Acme.Todoist.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Acme.Todoist.Data.Mappings;

internal sealed class TodoMapper : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("todo");

        builder.HasKey(it => it.Id);
        builder.Property(x => x.Id).HasColumnName("todo_id");

        builder.Property(it => it.Labels)
            .HasConversion(
                labels => JsonConvert.SerializeObject(labels),
                labels => JsonConvert.DeserializeObject<ICollection<string>>(labels));
    }
}
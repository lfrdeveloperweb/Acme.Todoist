using Acme.Todoist.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acme.Todoist.Data.Mappings;

internal sealed class TodoMapper : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("todo");

        builder.HasKey(it => it.Id);
        builder.Property(x => x.Id).HasColumnName("todo_id");
    }
}
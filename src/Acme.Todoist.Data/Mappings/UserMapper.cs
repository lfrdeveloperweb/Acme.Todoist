using Acme.Todoist.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acme.Todoist.Data.Mappings;

internal sealed class UserMapper : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey(it => it.Id);
        builder.Property(x => x.Id).HasColumnName("user_id")
            .ValueGeneratedNever();
    }
}
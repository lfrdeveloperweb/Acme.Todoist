using Acme.Todoist.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acme.Todoist.Data.Mappings;

internal sealed class UserMapper : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey(it => it.Id);
        builder.Property(x => x.Id)
            .HasColumnName("user_id")
            .ValueGeneratedNever();

        builder.Property(x => x.DocumentNumber)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.Property(x => x.UserName)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Role)
            .HasColumnName("role_id");
        
        builder.OwnsOne(it => it.CreatedBy, it =>
        {
            it.Property(x => x.Id)
                .HasColumnName("created_by_id")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            it.Property(x => x.Name)
                .HasColumnName("created_by_name")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        });

        builder.Property(x => x.CreatedAt)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.OwnsOne(it => it.UpdatedBy, it =>
        {
            it.Property(x => x.Id)
                .HasColumnName("updated_by_id")
                .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            it.Property(x => x.Name)
                .HasColumnName("updated_by_name")
                .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
        });

        builder.Property(x => x.UpdatedAt)
            .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriorAuthorization.Shared.Entities;

namespace PriorAuthorization.Shared.Configurations;

public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
{
    public void Configure(EntityTypeBuilder<Facility> builder)
    {
        builder.ToTable("Facility");

        builder.HasKey(x => x.FacilityId);

        builder.Property(x => x.FacilityName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.FacilityLocation)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.IsActive)
               .HasDefaultValue(true);

        builder.Property(x => x.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");
    }
}
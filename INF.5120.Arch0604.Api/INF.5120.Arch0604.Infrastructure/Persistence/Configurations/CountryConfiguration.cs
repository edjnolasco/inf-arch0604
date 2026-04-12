using INF._5120.Arch0604.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace INF._5120.Arch0604.Infrastructure.Persistence.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Country");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Description)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(c => c.IsoNum)
                   .IsRequired();

            builder.Property(c => c.IsoA2)
                   .IsRequired()
                   .HasMaxLength(2);

            builder.Property(c => c.IsoA3)
                   .IsRequired()
                   .HasMaxLength(3);

            builder.Property(c => c.IsEnable)
                   .IsRequired();

            builder.Property(c => c.CreatedDate)
                   .IsRequired();

            builder.Property(c => c.UpdatedDate);

            builder.HasIndex(c => c.Description).IsUnique();
            builder.HasIndex(c => c.IsoNum).IsUnique();
            builder.HasIndex(c => c.IsoA2).IsUnique();
            builder.HasIndex(c => c.IsoA3).IsUnique();
        }
    }
}
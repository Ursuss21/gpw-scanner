using MarketRadar.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketRadar.Infrastructure.Persistence.Configurations;

public sealed class InstrumentConfiguration : IEntityTypeConfiguration<Instrument>
{
    public void Configure(EntityTypeBuilder<Instrument> builder)
    {
        builder.ToTable("Instrument");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Symbol)
            .HasColumnType("varchar")
            .IsRequired();

        builder.HasIndex(x => x.Symbol)
            .IsUnique();
    }
}
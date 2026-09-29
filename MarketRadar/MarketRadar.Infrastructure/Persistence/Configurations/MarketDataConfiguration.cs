using MarketRadar.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketRadar.Infrastructure.Persistence.Configurations;

public sealed class MarketDataConfiguration : IEntityTypeConfiguration<MarketData>
{
    public void Configure(EntityTypeBuilder<MarketData> builder)
    {
        builder.ToTable("MarketData");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.AvailableAt)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(x => x.Price)
            .HasColumnType("decimal")
            .IsRequired();

        builder.Property(x => x.SessionTurnover)
            .HasColumnType("decimal")
            .IsRequired();

        builder.HasOne(x => x.Instrument)
            .WithMany()
            .HasForeignKey(x => x.InstrumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.InstrumentId,
            x.AvailableAt
        })
        .IsUnique();
    }
}
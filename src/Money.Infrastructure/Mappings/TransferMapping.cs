using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Money.Domain.Entities;

namespace Money.Infrastructure.Mappings;

public class TransferMapping : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.ToTable("transfers");

        builder.Property(x => x.FromParticipantId).HasColumnName("from_participant_id");
        builder.Property(x => x.ToParticipantId).HasColumnName("to_participant_id");

        builder.HasOne(e => e.FromParticipant)
            .WithOne()
            .HasForeignKey<Transfer>(e => e.FromParticipantId);

        builder.HasOne(e => e.ToParticipant)
            .WithOne()
            .HasForeignKey<Transfer>(e => e.ToParticipantId);
    }
}
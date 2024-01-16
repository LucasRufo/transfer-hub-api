using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Money.Domain.Entities;

namespace Money.Infrastructure.Mappings;

public class TransactionMapping : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.Property(x => x.Type).HasColumnName("transaction_type_id");
        builder.Property(x => x.ParticipantId).HasColumnName("participant_id");

        builder.HasOne(e => e.Participant)
            .WithMany(e => e.Transactions)
            .HasForeignKey(e => e.ParticipantId)
            .HasPrincipalKey(e => e.Id);

        builder.HasOne(e => e.Transfer)
            .WithOne(e => e.Transaction)
            .HasForeignKey<Transaction>(e => e.TransferId)
            .HasPrincipalKey<Transfer>(e => e.Id);
    }
}
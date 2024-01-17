using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TransferHub.Domain.Entities;

namespace TransferHub.Infrastructure.Mappings;

public class ParticipantMapping : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("participants");
    }
}

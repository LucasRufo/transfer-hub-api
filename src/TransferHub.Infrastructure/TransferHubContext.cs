using Microsoft.EntityFrameworkCore;
using TransferHub.Domain.Entities;
using System.Reflection;

namespace TransferHub.Infrastructure;

public class TransferHubContext : DbContext
{
    public TransferHubContext() { }

    public TransferHubContext(DbContextOptions<TransferHubContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql().UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Participant> Participant { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<Transfer> Transfer { get; set; }
}

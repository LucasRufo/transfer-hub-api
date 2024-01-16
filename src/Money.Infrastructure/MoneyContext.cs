using Microsoft.EntityFrameworkCore;
using Money.Domain.Entities;
using System.Reflection;

namespace Money.Infrastructure;

public class MoneyContext : DbContext
{
    public MoneyContext() { }

    public MoneyContext(DbContextOptions<MoneyContext> options) : base(options)
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

using HouseholdApp.Client.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseholdApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TransactionItem> Transactions => Set<TransactionItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransactionItem>(entity =>
        {
            entity.Property(t => t.Category).HasMaxLength(50).IsRequired();
            entity.Property(t => t.Type).HasConversion<string>();
        });
    }
}

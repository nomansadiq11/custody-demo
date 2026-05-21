using CustodyManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CustodyManagementApi.Data;

public class CustodyDbContext(DbContextOptions<CustodyDbContext> options) : DbContext(options)
{
    public DbSet<CustodyRecord> CustodyRecords => Set<CustodyRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CustodyRecord>();

        entity.ToTable("custody_records");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.PersonName).HasMaxLength(200).IsRequired();
        entity.Property(x => x.CaseNumber).HasMaxLength(100).IsRequired();
        entity.Property(x => x.Facility).HasMaxLength(200).IsRequired();
        entity.Property(x => x.Status).HasMaxLength(100).IsRequired();
        entity.Property(x => x.ArrestedAtUtc).IsRequired();
        entity.Property(x => x.CreatedAtUtc).IsRequired();
        entity.Property(x => x.UpdatedAtUtc).IsRequired();
        entity.HasIndex(x => x.CaseNumber).IsUnique();
    }
}

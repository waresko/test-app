using Microsoft.EntityFrameworkCore;
using WebApplication2.Entities;

namespace WebApplication2;

public class ApplicationDbContext : DbContext
{
    public DbSet<ProductionFacility> ProductionFacilities { get; set; }
    public DbSet<ProcessEquipmentType> ProcessEquipmentTypes { get; set; }
    public DbSet<EquipmentPlacementContract> EquipmentPlacementContracts { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure ProductionFacility
        modelBuilder.Entity<ProductionFacility>().HasKey(p => p.Code);

        // Configure ProcessEquipmentType
        modelBuilder.Entity<ProcessEquipmentType>().HasKey(e => e.Code);

        // Configure EquipmentPlacementContract
        modelBuilder.Entity<EquipmentPlacementContract>().HasKey(c => c.Id);

        modelBuilder.Entity<EquipmentPlacementContract>()
            .HasOne(c => c.ProductionFacility)
            .WithMany()
            .HasForeignKey(c => c.ProductionFacilityCode);

        modelBuilder.Entity<EquipmentPlacementContract>()
            .HasOne(c => c.ProcessEquipmentType)
            .WithMany()
            .HasForeignKey(c => c.ProcessEquipmentTypeCode);
    }
}

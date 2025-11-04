namespace MaintenanceTracker.Mvp.Data;


using MaintenanceTracker.Mvp.Domain;
using Microsoft.EntityFrameworkCore;


public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();


    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Asset>().Property(a => a.Name).HasMaxLength(120).IsRequired();
        b.Entity<WorkOrder>().Property(w => w.Title).HasMaxLength(200).IsRequired();
        b.Entity<WorkOrder>().HasOne(w => w.Asset).WithMany(a => a.WorkOrders).HasForeignKey(w => w.AssetId);
    }
}
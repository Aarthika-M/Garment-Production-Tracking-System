using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<GarmentOrder> GarmentOrders { get; set; }
        public DbSet<OrderAssignment> OrderAssignments { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }


     protected override void OnModelCreating(ModelBuilder modelBuilder)//table mapping,relationship
{
    // Set default schema to 'aa'
    modelBuilder.HasDefaultSchema("aa");

    // Convert enum Role to string
    modelBuilder.Entity<User>()
        .Property(u => u.Role)
        .HasConversion<string>();

    base.OnModelCreating(modelBuilder);//custom configurations, EF Core runs its default setup to finalize the model correctly
}


    }
}

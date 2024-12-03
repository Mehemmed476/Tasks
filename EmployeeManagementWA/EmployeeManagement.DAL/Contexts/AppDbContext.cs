using EmployeeManagement.CORE.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.DAL.Contexts;

public class AppDbContext : IdentityDbContext<AppUser>
{
    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Server=localhost;Database=EmployeeDB;User ID=SA; Password=reallyStrongPwd123;Trusted_Connection=False;Encrypt=True;TrustServerCertificate=True");
    }*/
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    DbSet<Service> Services { get; set; }
    DbSet<Master> Masters { get; set; }
    DbSet<Order> Orders { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Master>()
            .HasOne(e => e.Service)
            .WithMany(e => e.Masters)
            .HasForeignKey(e => e.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Order>()
            .HasOne(e => e.Master)
            .WithMany(e => e.Orders)
            .HasForeignKey(e => e.MasterId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Order>()
            .HasOne(e => e.Service)
            .WithMany(e => e.Orders)
            .HasForeignKey(e => e.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
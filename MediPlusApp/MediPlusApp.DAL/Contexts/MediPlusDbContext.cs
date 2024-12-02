using MediPlusApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace MediPlusApp.DAL.Contexts;

public class MediPlusDbContext : DbContext
{
    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Server=localhost;Database=MediPlusDB;User ID=SA; Password=reallyStrongPwd123;Trusted_Connection=False;Encrypt=True;TrustServerCertificate=True");
    }*/
    public MediPlusDbContext(DbContextOptions<MediPlusDbContext> options) : base(options) { }
    
    
    public DbSet<SliderItem> SliderItems { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Appointment>()
            .HasOne(e => e.Doctor)
            .WithMany(e => e.Appointments)
            .HasForeignKey(e => e.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Appointment>()
            .HasOne(e => e.Patient)
            .WithMany(e => e.Appointments)
            .HasForeignKey(e => e.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    
}
using Microsoft.EntityFrameworkCore;
using Scheduling.Domain.Entities;

namespace Scheduling.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<ServiceOffering> ServiceOfferings => Set<ServiceOffering>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<WorkingHours> WorkingHours => Set<WorkingHours>();
    public DbSet<BreakTime> BreakTimes => Set<BreakTime>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).HasMaxLength(128).IsRequired();
            entity.Property(c => c.Email).HasMaxLength(256).IsRequired();
            entity.HasIndex(c => c.Email).IsUnique();
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).HasMaxLength(128).IsRequired();
            entity.Property(s => s.Email).HasMaxLength(256).IsRequired();
            entity.HasIndex(s => s.Email).IsUnique();

            entity.HasMany(s => s.WorkingHours)
                .WithOne()
                .HasForeignKey(w => w.StaffId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(s => s.BreakTimes)
                .WithOne()
                .HasForeignKey(b => b.StaffId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Navigation(s => s.WorkingHours).HasField("_workingHours");
            entity.Navigation(s => s.BreakTimes).HasField("_breakTimes");
        });

        modelBuilder.Entity<WorkingHours>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.DayOfWeek).HasConversion<string>();
        });

        modelBuilder.Entity<BreakTime>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.DayOfWeek).HasConversion<string>();
        });

        modelBuilder.Entity<ServiceOffering>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).HasMaxLength(128).IsRequired();
            entity.Property(s => s.Price).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Status).HasConversion<string>();

            entity.HasOne<Customer>(a => a.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Staff>(a => a.Staff)
                .WithMany()
                .HasForeignKey(a => a.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<ServiceOffering>(a => a.ServiceOffering)
                .WithMany()
                .HasForeignKey(a => a.ServiceOfferingId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(a => new { a.StaffId, a.StartTime });
        });
    }
}

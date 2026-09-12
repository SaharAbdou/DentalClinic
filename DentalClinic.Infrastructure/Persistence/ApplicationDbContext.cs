using DentalClinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DentalClinic.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<DoctorWeeklyAvailability> DoctorWeeklyAvailabilities => Set<DoctorWeeklyAvailability>();
    public DbSet<DoctorScheduleException> DoctorScheduleExceptions => Set<DoctorScheduleException>();
    public DbSet<BeforeAfterPhoto> BeforeAfterPhotos => Set<BeforeAfterPhoto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<User> Users => Set<User>();
}
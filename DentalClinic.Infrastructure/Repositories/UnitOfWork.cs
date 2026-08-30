using DentalClinic.Application.Interfaces;
using DentalClinic.Domain.Entities;
using DentalClinic.Infrastructure.Persistence;

namespace DentalClinic.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IRepository<Doctor> Doctors { get; }
    public IRepository<Patient> Patients { get; }
    public IRepository<Appointment> Appointments { get; }
    public IRepository<DoctorWeeklyAvailability> DoctorWeeklyAvailabilities { get; }
    public IRepository<DoctorScheduleException> DoctorScheduleExceptions { get; }
    public IRepository<BeforeAfterPhoto> BeforeAfterPhotos { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Doctors = new Repository<Doctor>(context);
        Patients = new Repository<Patient>(context);
        Appointments = new Repository<Appointment>(context);
        DoctorWeeklyAvailabilities = new Repository<DoctorWeeklyAvailability>(context);
        DoctorScheduleExceptions = new Repository<DoctorScheduleException>(context);
        BeforeAfterPhotos = new Repository<BeforeAfterPhoto>(context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
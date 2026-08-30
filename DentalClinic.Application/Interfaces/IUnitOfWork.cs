using DentalClinic.Domain.Entities;

namespace DentalClinic.Application.Interfaces;

public interface IUnitOfWork
{
    IRepository<Doctor> Doctors { get; }
    IRepository<Patient> Patients { get; }
    IRepository<Appointment> Appointments { get; }
    IRepository<DoctorWeeklyAvailability> DoctorWeeklyAvailabilities { get; }
    IRepository<DoctorScheduleException> DoctorScheduleExceptions { get; }
    IRepository<BeforeAfterPhoto> BeforeAfterPhotos { get; }

    Task<int> SaveChangesAsync();
}
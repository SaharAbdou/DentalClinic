using DentalClinic.Domain.Common;
using DentalClinic.Domain.Enums;

namespace DentalClinic.Domain.Entities;

public class Doctor : BaseEntity
{
    public string Name { get; private set; } = null!;
    public DoctorSpecialty Specialty { get; private set; }
    public string? Bio { get; private set; }

    public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();
    public ICollection<BeforeAfterPhoto> Photos { get; private set; } = new List<BeforeAfterPhoto>();
    public ICollection<DoctorWeeklyAvailability> WeeklyAvailability { get; private set; } = new List<DoctorWeeklyAvailability>();
    public ICollection<DoctorScheduleException> ScheduleExceptions { get; private set; } = new List<DoctorScheduleException>();

    private Doctor() { } // EF Core

    public Doctor(string name, DoctorSpecialty specialty, string? bio = null)
    {
        Name = name;
        Specialty = specialty;
        Bio = bio;
    }

    public void UpdateProfile(string name, string? bio)
    {
        Name = name;
        Bio = bio;
    }
}
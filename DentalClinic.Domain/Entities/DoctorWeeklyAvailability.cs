using DentalClinic.Domain.Common;

namespace DentalClinic.Domain.Entities;

public class DoctorWeeklyAvailability : BaseEntity
{
    public Guid DoctorId { get; private set; }
    public Doctor Doctor { get; private set; } = null!;

    public DayOfWeek DayOfWeek { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    private DoctorWeeklyAvailability() { } // EF Core

    public DoctorWeeklyAvailability(Guid doctorId, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
    {
        DoctorId = doctorId;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
    }
}
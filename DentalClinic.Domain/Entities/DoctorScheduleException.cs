using DentalClinic.Domain.Common;
using DentalClinic.Domain.Enums;

namespace DentalClinic.Domain.Entities;

public class DoctorScheduleException : BaseEntity
{
    public Guid DoctorId { get; private set; }
    public Doctor Doctor { get; private set; } = null!;

    public DateOnly Date { get; private set; }
    public ScheduleExceptionType Type { get; private set; }
    public TimeOnly? ModifiedStartTime { get; private set; }
    public TimeOnly? ModifiedEndTime { get; private set; }

    private DoctorScheduleException() { } // EF Core

    public DoctorScheduleException(Guid doctorId, DateOnly date, ScheduleExceptionType type,
        TimeOnly? modifiedStartTime = null, TimeOnly? modifiedEndTime = null)
    {
        DoctorId = doctorId;
        Date = date;
        Type = type;
        ModifiedStartTime = modifiedStartTime;
        ModifiedEndTime = modifiedEndTime;
    }
}
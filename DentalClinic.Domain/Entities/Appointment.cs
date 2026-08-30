using DentalClinic.Domain.Common;
using DentalClinic.Domain.Enums;

namespace DentalClinic.Domain.Entities;

public class Appointment : BaseEntity
{
    public Guid DoctorId { get; private set; }
    public Doctor Doctor { get; private set; } = null!;

    public Guid PatientId { get; private set; }
    public Patient Patient { get; private set; } = null!;

    public AppointmentType Type { get; private set; }
    public DateTime ScheduledAt { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public string? Notes { get; private set; }

    private Appointment() { } // EF Core

    public Appointment(Guid patientId, Guid doctorId, AppointmentType type, DateTime scheduledAt, string? notes = null)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        Type = type;
        ScheduledAt = scheduledAt;
        Notes = notes;
        Status = AppointmentStatus.Pending;
    }

    public void Confirm() => Status = AppointmentStatus.Confirmed;
    public void Complete() => Status = AppointmentStatus.Completed;
    public void Cancel() => Status = AppointmentStatus.Cancelled;
}
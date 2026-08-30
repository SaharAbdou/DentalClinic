using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DentalClinic.Application.DTOs.Appointment;

public class CreateAppointmentDto
{
    public Guid DoctorId { get; set; }
    public string PatientName { get; set; } = null!;
    public string PatientPhoneNumber { get; set; } = null!;
    public string AppointmentType { get; set; } = null!; // "Checkup", "TreatmentSession", "OrthodonticFollowUp"
    public DateTime ScheduledAt { get; set; }
    public string? Notes { get; set; }
}

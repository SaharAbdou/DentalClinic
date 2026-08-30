using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DentalClinic.Application.DTOs.Appointment;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public string DoctorName { get; set; } = null!;
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = null!;
    public string AppointmentType { get; set; } = null!;
    public DateTime ScheduledAt { get; set; }
    public string Status { get; set; } = null!;
}

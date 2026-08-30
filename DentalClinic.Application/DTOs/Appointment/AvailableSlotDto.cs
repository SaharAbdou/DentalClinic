using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DentalClinic.Application.DTOs.Appointment;

public class AvailableSlotDto
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.Application.DTOs.Schedule;

public class CreateExceptionDto
{
    public DateOnly Date { get; set; }
    public string Type { get; set; } = null!; // "DayOff" أو "ModifiedHours"
    public TimeOnly? ModifiedStartTime { get; set; }
    public TimeOnly? ModifiedEndTime { get; set; }
}

public class ExceptionDto
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public string Type { get; set; } = null!;
    public TimeOnly? ModifiedStartTime { get; set; }
    public TimeOnly? ModifiedEndTime { get; set; }
}
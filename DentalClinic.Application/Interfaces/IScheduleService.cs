using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DentalClinic.Application.DTOs.Appointment;

namespace DentalClinic.Application.Interfaces;

public interface IScheduleService
{
    Task<IReadOnlyList<AvailableSlotDto>> GetAvailableSlotsAsync(Guid doctorId, DateOnly date);
    Task<IReadOnlyList<AvailableDayDto>> GetAvailableDaysAsync(Guid doctorId, int year, int month);
}

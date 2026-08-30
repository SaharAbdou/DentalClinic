using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalClinic.Application.DTOs.Schedule;

namespace DentalClinic.Application.Interfaces;

public interface IDoctorAvailabilityService
{
    Task AddWeeklyAvailabilityAsync(Guid doctorId, CreateWeeklyAvailabilityDto dto);
}
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
    Task UpdateWeeklyAvailabilityAsync(Guid id, CreateWeeklyAvailabilityDto dto);
    Task DeleteWeeklyAvailabilityAsync(Guid id);
    Task<IReadOnlyList<ExceptionDto>> GetExceptionsAsync(Guid doctorId);
    Task<ExceptionDto> AddExceptionAsync(Guid doctorId, CreateExceptionDto dto);
    Task DeleteExceptionAsync(Guid id);
}
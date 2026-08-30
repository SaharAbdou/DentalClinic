using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DentalClinic.Application.DTOs.Appointment;

namespace DentalClinic.Application.Interfaces;

public interface IAppointmentService
{
    Task<IReadOnlyList<AppointmentDto>> GetByPhoneNumberAsync(string phoneNumber);
    Task CancelAsync(Guid appointmentId);
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
}
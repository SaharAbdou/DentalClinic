using DentalClinic.Application.DTOs.Appointment;
using DentalClinic.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalClinic.Domain.Enums;
namespace DentalClinic.Application.Interfaces;

public interface IAppointmentService
{
    Task<IReadOnlyList<AppointmentDto>> GetByPhoneNumberAsync(string phoneNumber);
    Task CancelAsync(Guid appointmentId);
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
    Task<IReadOnlyList<AppointmentDto>> GetAllAsync(AppointmentStatus? status, Guid? doctorId, DateOnly? date);
    Task ConfirmAsync(Guid appointmentId);
    Task CompleteAsync(Guid appointmentId);
}
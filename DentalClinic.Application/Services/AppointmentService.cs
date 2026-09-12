using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalClinic.Application.DTOs.Appointment;
using DentalClinic.Application.Interfaces;
using DentalClinic.Domain.Entities;
using DentalClinic.Domain.Enums;

namespace DentalClinic.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        // 1. تأكيد إن الدكتور موجود
        var doctor = await _unitOfWork.Doctors.GetByIdAsync(dto.DoctorId);
        if (doctor == null)
            throw new ArgumentException("Doctor not found");

        // 2. تأكيد إن الـ slot لسه متاح (مفيش حجز تاني عليه)
        var conflictingAppointments = await _unitOfWork.Appointments.FindAsync(
            a => a.DoctorId == dto.DoctorId
                 && a.ScheduledAt == dto.ScheduledAt
                 && a.Status != AppointmentStatus.Cancelled);

        if (conflictingAppointments.Any())
            throw new InvalidOperationException("This time slot is no longer available");

        // 3. البحث عن المريض بالـ phone number، أو إنشاء واحد جديد
        var existingPatients = await _unitOfWork.Patients.FindAsync(
            p => p.PhoneNumber == dto.PatientPhoneNumber);

        var patient = existingPatients.FirstOrDefault();

        if (patient == null)
        {
            patient = new Patient(dto.PatientName, dto.PatientPhoneNumber);
            await _unitOfWork.Patients.AddAsync(patient);
        }
        else if (patient.Name != dto.PatientName)
        {
            patient.UpdateName(dto.PatientName);
            _unitOfWork.Patients.Update(patient);
        }

        // 4. تحويل نوع الحجز من string لـ enum
        if (!Enum.TryParse<AppointmentType>(dto.AppointmentType, ignoreCase: true, out var appointmentType))
            throw new ArgumentException($"Invalid appointment type: {dto.AppointmentType}");

        // 5. إنشاء الحجز
        var appointment = new Appointment(patient.Id, dto.DoctorId, appointmentType, dto.ScheduledAt, dto.Notes);
        await _unitOfWork.Appointments.AddAsync(appointment);

        // 6. حفظ كل حاجة مع بعض (Patient + Appointment) — هنا بالظبط فايدة الـ UnitOfWork
        await _unitOfWork.SaveChangesAsync();

        return new AppointmentDto
        {
            Id = appointment.Id,
            DoctorId = doctor.Id,
            DoctorName = doctor.Name,
            PatientId = patient.Id,
            PatientName = patient.Name,
            AppointmentType = appointment.Type.ToString(),
            ScheduledAt = appointment.ScheduledAt,
            Status = appointment.Status.ToString()
        };
    }
    public async Task<IReadOnlyList<AppointmentDto>> GetByPhoneNumberAsync(string phoneNumber)
    {
        var patients = await _unitOfWork.Patients.FindAsync(p => p.PhoneNumber == phoneNumber);
        var patient = patients.FirstOrDefault();

        if (patient == null)
            return new List<AppointmentDto>();

        var appointments = await _unitOfWork.Appointments.FindAsync(a => a.PatientId == patient.Id);

        var result = new List<AppointmentDto>();
        foreach (var appointment in appointments.OrderByDescending(a => a.ScheduledAt))
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(appointment.DoctorId);

            result.Add(new AppointmentDto
            {
                Id = appointment.Id,
                DoctorId = appointment.DoctorId,
                DoctorName = doctor?.Name ?? "غير معروف",
                PatientId = patient.Id,
                PatientName = patient.Name,
                AppointmentType = appointment.Type.ToString(),
                ScheduledAt = appointment.ScheduledAt,
                Status = appointment.Status.ToString()
            });
        }

        return result;
    }

    public async Task CancelAsync(Guid appointmentId)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);
        if (appointment == null)
            throw new ArgumentException("Appointment not found");

        if (appointment.Status == AppointmentStatus.Cancelled)
            throw new InvalidOperationException("Appointment is already cancelled");

        appointment.Cancel();
        _unitOfWork.Appointments.Update(appointment);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<IReadOnlyList<AppointmentDto>> GetAllAsync(AppointmentStatus? status, Guid? doctorId, DateOnly? date)
    {
        var appointments = await _unitOfWork.Appointments.FindAsync(a =>
            (!status.HasValue || a.Status == status.Value) &&
            (!doctorId.HasValue || a.DoctorId == doctorId.Value) &&
            (!date.HasValue || a.ScheduledAt.Date == date.Value.ToDateTime(TimeOnly.MinValue).Date));

        var result = new List<AppointmentDto>();
        foreach (var appointment in appointments.OrderByDescending(a => a.ScheduledAt))
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(appointment.DoctorId);
            var patient = await _unitOfWork.Patients.GetByIdAsync(appointment.PatientId);

            result.Add(new AppointmentDto
            {
                Id = appointment.Id,
                DoctorId = appointment.DoctorId,
                DoctorName = doctor?.Name ?? "غير معروف",
                PatientId = appointment.PatientId,
                PatientName = patient?.Name ?? "غير معروف",
                AppointmentType = appointment.Type.ToString(),
                ScheduledAt = appointment.ScheduledAt,
                Status = appointment.Status.ToString()
            });
        }
        return result;
    }

    public async Task ConfirmAsync(Guid appointmentId)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId)
            ?? throw new ArgumentException("Appointment not found");
        appointment.Confirm();
        _unitOfWork.Appointments.Update(appointment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CompleteAsync(Guid appointmentId)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId)
            ?? throw new ArgumentException("Appointment not found");
        appointment.Complete();
        _unitOfWork.Appointments.Update(appointment);
        await _unitOfWork.SaveChangesAsync();
    }
}
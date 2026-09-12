using DentalClinic.Application.DTOs.Schedule;
using DentalClinic.Application.Interfaces;
using DentalClinic.Domain.Entities;
using DentalClinic.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DentalClinic.Application.Services;

public class DoctorAvailabilityService : IDoctorAvailabilityService
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorAvailabilityService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddWeeklyAvailabilityAsync(Guid doctorId, CreateWeeklyAvailabilityDto dto)
    {
        var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);
        if (doctor == null)
            throw new ArgumentException("Doctor not found");

        var availability = new DoctorWeeklyAvailability(doctorId, dto.DayOfWeek, dto.StartTime, dto.EndTime);

        await _unitOfWork.DoctorWeeklyAvailabilities.AddAsync(availability);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task UpdateWeeklyAvailabilityAsync(Guid id, CreateWeeklyAvailabilityDto dto)
    {
        var availability = await _unitOfWork.DoctorWeeklyAvailabilities.GetByIdAsync(id)
            ?? throw new ArgumentException("Availability not found");
        availability.Update(dto.DayOfWeek, dto.StartTime, dto.EndTime);
        _unitOfWork.DoctorWeeklyAvailabilities.Update(availability);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteWeeklyAvailabilityAsync(Guid id)
    {
        var availability = await _unitOfWork.DoctorWeeklyAvailabilities.GetByIdAsync(id)
            ?? throw new ArgumentException("Availability not found");
        _unitOfWork.DoctorWeeklyAvailabilities.Remove(availability);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<IReadOnlyList<ExceptionDto>> GetExceptionsAsync(Guid doctorId)
    {
        var exceptions = await _unitOfWork.DoctorScheduleExceptions.FindAsync(e => e.DoctorId == doctorId);
        return exceptions.Select(e => new ExceptionDto
        {
            Id = e.Id,
            Date = e.Date,
            Type = e.Type.ToString(),
            ModifiedStartTime = e.ModifiedStartTime,
            ModifiedEndTime = e.ModifiedEndTime
        }).ToList();
    }

    public async Task<ExceptionDto> AddExceptionAsync(Guid doctorId, CreateExceptionDto dto)
    {
        if (!Enum.TryParse<ScheduleExceptionType>(dto.Type, true, out var type))
            throw new ArgumentException($"Invalid type: {dto.Type}");

        var exception = new DoctorScheduleException(doctorId, dto.Date, type, dto.ModifiedStartTime, dto.ModifiedEndTime);
        await _unitOfWork.DoctorScheduleExceptions.AddAsync(exception);
        await _unitOfWork.SaveChangesAsync();

        return new ExceptionDto
        {
            Id = exception.Id,
            Date = exception.Date,
            Type = exception.Type.ToString(),
            ModifiedStartTime = exception.ModifiedStartTime,
            ModifiedEndTime = exception.ModifiedEndTime
        };
    }

    public async Task DeleteExceptionAsync(Guid id)
    {
        var exception = await _unitOfWork.DoctorScheduleExceptions.GetByIdAsync(id)
            ?? throw new ArgumentException("Exception not found");
        _unitOfWork.DoctorScheduleExceptions.Remove(exception);
        await _unitOfWork.SaveChangesAsync();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalClinic.Application.DTOs.Schedule;
using DentalClinic.Application.Interfaces;
using DentalClinic.Domain.Entities;

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
}
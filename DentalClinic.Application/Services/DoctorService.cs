using DentalClinic.Application.DTOs.Doctor;
using DentalClinic.Application.Interfaces;
using DentalClinic.Domain.Entities;
using DentalClinic.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<DoctorDto>> GetAllAsync()
    {
        var doctors = await _unitOfWork.Doctors.GetAllAsync();

        return doctors.Select(d => new DoctorDto
        {
            Id = d.Id,
            Name = d.Name,
            Specialty = d.Specialty.ToString(),
            Bio = d.Bio
        }).ToList();
    }

    public async Task<DoctorDto?> GetByIdAsync(Guid id)
    {
        var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
        if (doctor == null) return null;

        return new DoctorDto
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Specialty = doctor.Specialty.ToString(),
            Bio = doctor.Bio
        };
    }
    public async Task<DoctorDto> CreateAsync(CreateDoctorDto dto)
    {
        if (!Enum.TryParse<DoctorSpecialty>(dto.Specialty, ignoreCase: true, out var specialty))
            throw new ArgumentException($"Invalid specialty: {dto.Specialty}");

        var doctor = new Doctor(dto.Name, specialty, dto.Bio);

        await _unitOfWork.Doctors.AddAsync(doctor);
        await _unitOfWork.SaveChangesAsync();

        return new DoctorDto
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Specialty = doctor.Specialty.ToString(),
            Bio = doctor.Bio
        };
    }
}
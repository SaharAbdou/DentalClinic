using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalClinic.Application.DTOs.Appointment;
using DentalClinic.Application.Interfaces;
using DentalClinic.Domain.Enums;

namespace DentalClinic.Application.Services;

public class ScheduleService : IScheduleService
{
    private const int SlotDurationMinutes = 30;

    private readonly IUnitOfWork _unitOfWork;

    public ScheduleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<AvailableSlotDto>> GetAvailableSlotsAsync(Guid doctorId, DateOnly date)
    {
        // 1. هل فيه استثناء في اليوم ده؟
        var exceptions = await _unitOfWork.DoctorScheduleExceptions.FindAsync(
            e => e.DoctorId == doctorId && e.Date == date);

        var exception = exceptions.FirstOrDefault();

        TimeOnly startTime, endTime;

        if (exception != null && exception.Type == ScheduleExceptionType.DayOff)
        {
            return new List<AvailableSlotDto>(); // يوم إجازة، مفيش أي slots
        }
        else if (exception != null && exception.Type == ScheduleExceptionType.ModifiedHours)
        {
            startTime = exception.ModifiedStartTime!.Value;
            endTime = exception.ModifiedEndTime!.Value;
        }
        else
        {
            // 2. مفيش استثناء، نجيب الجدول الأساسي لليوم ده
            var weeklyAvailability = await _unitOfWork.DoctorWeeklyAvailabilities.FindAsync(
                w => w.DoctorId == doctorId && w.DayOfWeek == date.DayOfWeek);

            var availability = weeklyAvailability.FirstOrDefault();
            if (availability == null)
                return new List<AvailableSlotDto>(); // الدكتور مش شغال اليوم ده أساسًا

            startTime = availability.StartTime;
            endTime = availability.EndTime;
        }

        // 3. نبني كل الـ slots الممكنة
        var allSlots = new List<AvailableSlotDto>();
        var current = startTime;

        while (current.AddMinutes(SlotDurationMinutes) <= endTime)
        {
            allSlots.Add(new AvailableSlotDto
            {
                StartTime = current,
                EndTime = current.AddMinutes(SlotDurationMinutes)
            });
            current = current.AddMinutes(SlotDurationMinutes);
        }

        // 4. نستبعد الـ slots المحجوزة فعلاً (أي حجز مش Cancelled)
        var bookedAppointments = await _unitOfWork.Appointments.FindAsync(
            a => a.DoctorId == doctorId
                 && a.ScheduledAt.Date == date.ToDateTime(TimeOnly.MinValue).Date
                 && a.Status != AppointmentStatus.Cancelled);

        var bookedTimes = bookedAppointments
            .Select(a => TimeOnly.FromDateTime(a.ScheduledAt))
            .ToHashSet();

        return allSlots.Where(s => !bookedTimes.Contains(s.StartTime)).ToList();
    }
    public async Task<IReadOnlyList<AvailableDayDto>> GetAvailableDaysAsync(Guid doctorId, int year, int month)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var availableDays = new List<AvailableDayDto>();

        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateOnly(year, month, day);

            // نتجاهل الأيام اللي فاتت
            if (date < DateOnly.FromDateTime(DateTime.Today))
                continue;

            var slots = await GetAvailableSlotsAsync(doctorId, date);
            if (slots.Any())
            {
                availableDays.Add(new AvailableDayDto { Date = date });
            }
        }

        return availableDays;
    }
}

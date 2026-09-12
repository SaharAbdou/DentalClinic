using Microsoft.AspNetCore.Mvc;
using DentalClinic.Application.Interfaces;
namespace DentalClinic.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet("available-slots")]
    public async Task<IActionResult> GetAvailableSlots([FromQuery] Guid doctorId, [FromQuery] DateOnly date)
    {
        var slots = await _scheduleService.GetAvailableSlotsAsync(doctorId, date);
        return Ok(slots);
    }
    [HttpGet("available-days")]
    public async Task<IActionResult> GetAvailableDays([FromQuery] Guid doctorId, [FromQuery] int year, [FromQuery] int month)
    {
        var days = await _scheduleService.GetAvailableDaysAsync(doctorId, year, month);
        return Ok(days);
    }
}
using Microsoft.AspNetCore.Mvc;
using Scheduling.Application.DTOs.Availability;
using Scheduling.Application.Services.Interfaces;

namespace Scheduling.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AvailabilityController : ControllerBase
{
    private readonly IAvailabilityService _availabilityService;

    public AvailabilityController(IAvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableSlots([FromQuery] AvailabilityQueryDto dto)
    {
        var result = await _availabilityService.GetAvailableSlotsAsync(dto);
        return Ok(result);
    }
}

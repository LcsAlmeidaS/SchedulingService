using Microsoft.AspNetCore.Mvc;
using Scheduling.Application.DTOs.ServiceOffering;
using Scheduling.Application.Services.Interfaces;

namespace Scheduling.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServiceOfferingsController : ControllerBase
{
    private readonly IServiceOfferingService _serviceOfferingService;

    public ServiceOfferingsController(IServiceOfferingService serviceOfferingService)
    {
        _serviceOfferingService = serviceOfferingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllActive()
    {
        var result = await _serviceOfferingService.GetAllActiveAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _serviceOfferingService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceOfferingDto dto)
    {
        var result = await _serviceOfferingService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateServiceOfferingDto dto)
    {
        var result = await _serviceOfferingService.UpdateAsync(id, dto);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate([FromRoute] Guid id)
    {
        var result = await _serviceOfferingService.ActivateAsync(id);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate([FromRoute] Guid id)
    {
        var result = await _serviceOfferingService.DeactivateAsync(id);
        return Ok(result);
    }
}

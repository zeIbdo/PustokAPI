using Microsoft.AspNetCore.Mvc;
using Pustok.Application.Dtos.ServiceDtos;
using Pustok.Application.Services.Abstractions;
using Pustok.Application.Services.Implementations;
using System.Threading.Tasks;

namespace Pustok.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly IServiceEntityService _serviceEntityService;

    public ServiceController(IServiceEntityService serviceEntityService)
    {
        _serviceEntityService = serviceEntityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetServices()
    {
        var services = await _serviceEntityService.GetAllAsync();
        return Ok(services);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateService([FromForm] ServiceCreateDto dto)
    {
        var resultId = await _serviceEntityService.CreateAsync(dto);
        return Ok(resultId);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(int id)
    {
        await _serviceEntityService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateService(int id, [FromForm] ServiceUpdateDto dto)
    {
        await _serviceEntityService.UpdateAsync(dto, id);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetService(int id)
    {
        var service = await _serviceEntityService.GetAsync(id);
        return Ok(service);
    }
}

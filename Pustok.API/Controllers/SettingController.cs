using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustok.Application.Dtos.SettingDtos;
using Pustok.Application.Services.Abstractions;
using System.Threading.Tasks;

namespace Pustok.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SettingController : ControllerBase
{
    private readonly ISettingService _settingService;

    public SettingController(ISettingService settingService)
    {
        _settingService = settingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSettings()
    {
        var settings = await _settingService.GetAllAsync();
        return Ok(settings);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> UpdateSetting(int id, [FromBody] SettingUpdateDto dto)
    {
        await _settingService.UpdateAsync(dto, id);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSetting(int id)
    {
        var setting =await _settingService.GetAsync(id);
        return Ok(setting);
    }
}

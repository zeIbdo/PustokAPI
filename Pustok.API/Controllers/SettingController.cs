using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("settings")]
    public async Task<IActionResult> GetSettings()
    {
        var settings  = await _settingService.GetAllAsync();
        return Ok(settings);
    }
}

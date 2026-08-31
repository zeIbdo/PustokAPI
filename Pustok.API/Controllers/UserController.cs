using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustok.Application.Dtos.AppUserDtos;
using Pustok.Application.Services.Abstractions;

namespace Pustok.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles ="Admin")]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;

    public UserController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _authService.GetUsersAsync();
        return Ok(users);
    }

    [HttpPatch("{id}/role")]
    public async Task<IActionResult> ChangeUserRole(string id,[FromBody] AppUserRoleChangeDto role)
    {
        await _authService.ChangeRoleAsync(id, role);
        return NoContent();
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeUserStatus(string id, [FromBody] AppUserStatusChangeDto userStatus)
    {
        var result = await _authService.ChangeStatusAsync(id, userStatus);
        if (!result)
            return BadRequest();
        return NoContent();
    }
}

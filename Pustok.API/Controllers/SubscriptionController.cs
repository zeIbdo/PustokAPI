using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustok.Application.Dtos.SubscriptionDtos;
using Pustok.Application.Services.Abstractions;
using Pustok.Application.Services.Implementations;

namespace Pustok.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSubscriptions()
    {
        var subscriptions = await _subscriptionService.GetAllAsync();
        return Ok(subscriptions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubscription(int id)
    {
        var subscription = await _subscriptionService.GetAsync(id);
        return Ok(subscription);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription([FromBody] SubscriptionCreateDto dto)
    {
        var subscriptionId = await _subscriptionService.CreateAsync(dto);
        return Ok(subscriptionId);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSubscription(int id)
    {
        await _subscriptionService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> UpdateSubscription(int id, [FromBody] SubscriptionUpdateDto dto)
    {
        await _subscriptionService.UpdateAsync(dto, id);
        return NoContent();
    }
}
